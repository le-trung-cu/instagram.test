using AutoMapper;
using Contracts;
using Entities.Exceptions;
using Entities.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Service.Contracts;
using Shared.DataTransferObjects;
using Shared.RequestFeatures;
using System.Security.Claims;

namespace Service
{
    public sealed class PostService : IPostService
    {
        private readonly IRepositoryManager _repository;
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;
        private readonly IAuthorizationService _authService;
        private readonly IFileService _fileService;
        private readonly IStoryService _storyService;
        public PostService(
            IRepositoryManager repository,
            IMapper mapper,
            UserManager<User> userManager,
            IAuthorizationService authService,
            IFileService fileService,
            ILoggerManager logger,
            IStoryService storyService)
        {
            _repository = repository;
            _logger = logger;
            _userManager = userManager;
            _mapper = mapper;
            _authService = authService;
            _fileService = fileService;
            _storyService = storyService;
        }

        public async Task<PostDto> CreatePostAsync(PostBaseForCreationDto postForCreation, ClaimsPrincipal user)
        {
            var ownerId = _userManager.GetUserId(user);
            var ownerName = _userManager.GetUserName(user);
            SaveFiles();
            await SetUserIdOfUserTags();
            var post = _mapper.Map<Post>(postForCreation);
            post.OwnerId = ownerId;

            _repository.Post.CreatePost(post);
            await _repository.SaveAsync();

            await CreateFeedPostForFriends();

            return _mapper.Map<PostDto>(post);

            async Task SetUserIdOfUserTags()
            {
                var usernameTaggeds = postForCreation.GetUserNamesInMediaItem();
                var userTaggeds = await _userManager.Users
                        .Where(t => usernameTaggeds.Contains(t.UserName))
                        .ToDictionaryAsync(t => t.UserName, t => t.Id);
                postForCreation.SetUserIdOfUserTags(userTaggeds);
            }
            void SaveFiles()
            {
                if (postForCreation is PostPhotoForCreationDto postPhotoDto)
                {
                    postPhotoDto.PhotoItem!.Path = _fileService.SaveImageOptimized(postPhotoDto!.PhotoItem!.File!);
                }
                else if (postForCreation is PostVideoForCreationDto postVideoDto)
                {
                    postVideoDto.VideoItem!.Path = _fileService.SaveFile(postVideoDto.VideoItem!.File!);
                }
                else if (postForCreation is PostCarouselForCreationDto postCarouselDto)
                {
                    if (postCarouselDto.VideoItems != null)
                        foreach (var item in postCarouselDto.VideoItems)
                        {
                            item.Path = _fileService.SaveFile(item.File!);
                        }
                    if (postCarouselDto.PhotoItems != null)
                        foreach (var item in postCarouselDto.PhotoItems)
                        {
                            item.Path = _fileService.SaveImageOptimized(item.File!);
                        }
                }
            }
            async Task CreateFeedPostForFriends()
            {
                var followers = await _repository.Follow.GetFollowersForUser(ownerId);
                await _storyService.SetStoryAsync(new StoryDto
                {
                    UserName = ownerName,
                    UserId = ownerId,
                    Avata = "",
                    PostId = post.Id,
                });
                foreach (var friend in followers)
                {
                    await _repository.FeePostCache.CreateFeedPostAsync(friend.UserName, new FeedPost(ownerId, post.Id));
                }
            }
        }

        public async Task DeletePostAsync(int postId, ClaimsPrincipal user)
        {
            var post = await _repository.Post.GetPostAsync(postId);
            if (post == null)
                throw new PostNotFoundException(postId);

            var userId = _userManager.GetUserId(user);
            var authResult = await _authService.AuthorizeAsync(user, post, "CanDeletePost");
            if (!authResult.Succeeded)
            {
                throw new PostDeleteForbiddenException(userId, postId);
            }

            _repository.Post.DeletedPost(post);
            await _repository.SaveAsync();
        }

        public async Task DeleteSavedPost(ClaimsPrincipal user, int postId)
        {
            var userId = _userManager.GetUserId(user);
            _repository.SavePost.DeleteSavedPost(new UserSavedPost { OwnerId = userId, PostId = postId });
            await _repository.SaveAsync();
        }

        public async Task<PostDto> GetPostAsync(int postId, ClaimsPrincipal? user)
        {
            var userId = user != null ? _userManager.GetUserId(user) : null;
            // if post hit cache
            PostDto? postDto = await _repository.PostCache.GetPostAsync(postId);
            bool? liked = userId != null ? await _repository.Like.CheckPostLiked(userId, postId) : null;
            bool? saved = userId != null ? await _repository.SavePost.IsSavedByUser(userId, postId) : null;
            if (postDto != null)
            {
                postDto.Saved = saved;
                postDto.Liked = liked;
                return postDto;
            }
            // if post not hit cache
            var post = await _repository.Post.GetPostAsync(postId);
            if (post == null)
            {
                throw new PostNotFoundException(postId);
            }
            postDto = await MapPostToPostDto(post);
            await SetUserNameForTaggedUser(postDto);
            await _repository.PostCache.CreatePostAsync(postDto);
            postDto.Saved = saved;
            postDto.Liked = liked;
            return postDto;
        }

        public async Task<(IEnumerable<PostDto> posts, MetaData metaData)> GetPostsAsync(string ownerName,
            PostParameters postParameters, ClaimsPrincipal? user)
        {
            var owner = await _userManager.FindByNameAsync(ownerName);
            if (owner == null)
                throw new UserNotFoundException(ownerName);

            if (postParameters == null)
                throw new NullParametersBadRequestException(nameof(postParameters));

            var userId = user != null ? _userManager.GetUserId(user) : null;
            var posts = await _repository.Post.GetPostsAsync(owner.Id, postParameters);

            IEnumerable<PostDto> postDtos = await MapPostToPostDtoIncludeMedia(posts);

            await SetLikedAndSavedForPost(userId, postDtos);
            await SetUserNameForTaggedUser(postDtos);
            return (postDtos, posts.MetaData);
        }

        private async Task<PostDto> MapPostToPostDto(Post post)
        {
            return (await MapPostToPostDtoIncludeMedia(new List<Post> { post })).Single();
        }

        private async Task SetUserNameForTaggedUser(PostDto postDto)
        {
            await SetUserNameForTaggedUser(new List<PostDto> { postDto });
        }

        private async Task SetUserNameForTaggedUser(IEnumerable<PostDto> postDtos)
        {
            var userTaggedIds = postDtos.SelectMany(t => t.MediaItems!)
                .SelectMany(t => t.UserTags!)
                .Select(t => t.UserId)
                .Distinct();

            var userNames = await _userManager.Users
                .Where(t => userTaggedIds.Contains(t.Id))
                .ToDictionaryAsync(user => user.Id, user => user.UserName);

            foreach (var post in postDtos)
            {
                if (post.MediaItems != null)
                    foreach (var mediaItem in post.MediaItems)
                    {
                        if (mediaItem.UserTags != null)
                            foreach (var tagged in mediaItem.UserTags)
                            {
                                if (userNames.TryGetValue(tagged.UserId!, out string? userName))
                                    tagged.UserName = userName;
                            }
                    }
            }
        }

        public async Task SetLikedAndSavedForPost(string? userId, IEnumerable<PostDto> postDtos)
        {
            var postIds = postDtos.Select(post => post.Id);

            var likedPostIds = userId != null ?
                await _repository.Like.FilterLikedPostIdsByUser(userId, postIds)
                : null;
            var savedPostIds = userId != null ?
               await _repository.SavePost.FilterSavedPostIdsByUser(userId, postIds)
               : null;
            foreach (var p in postDtos)
            {
                p.Liked = likedPostIds?.Contains(p.Id);
                p.Saved = savedPostIds?.Contains(p.Id);
            }
        }

        public async Task SavePost(ClaimsPrincipal user, int postId)
        {
            var userId = _userManager.GetUserId(user);
            _repository.SavePost.SavePost(new UserSavedPost { OwnerId = userId, PostId = postId });
            await _repository.SaveAsync();
        }

        public async Task UpdatePostAsync(int postId, PostForUpdateDto postForUpdate)
        {
            var postFromDatabase = await _repository.Post.GetPostAsync(postId, trackChanges: true);
            if (postFromDatabase == null)
                throw new PostNotFoundException(postId);
            _mapper.Map(postForUpdate, postFromDatabase);
            await _repository.SaveAsync();
        }

        public async Task<(IEnumerable<PostThumbnail> posts, MetaData metaData)> GetSavedPostsOfUser(ClaimsPrincipal user, PostParameters postParameters)
        {
            var userId = _userManager.GetUserId(user);
            var savedPosts = await _repository.SavePost.GetSavedPostOfUser(userId, postParameters);
            var posts = savedPosts.Select(t => t.Post);
            var postDtos = _mapper.Map<IEnumerable<PostThumbnail>>(posts);
            return (postDtos, savedPosts.MetaData);
        }

        private async Task<IEnumerable<PostDto>> MapPostToPostDtoIncludeMedia(IEnumerable<Post> posts)
        {
            var postDtos = _mapper.Map<IEnumerable<PostDto>>(posts);
            var postIds = posts.Select(post => post.Id);
            var mediaItemOfPosts = await _repository.Media.GetMediaItemsOfPosts(postIds);
            foreach (var postDto in postDtos)
            {
                if (mediaItemOfPosts.TryGetValue(postDto.Id, out IEnumerable<MediaItem>? mediaItems))
                    postDto.MediaItems = _mapper.Map<IEnumerable<MediaItemDto>>(mediaItems);
            }

            return postDtos;
        }

        public async Task<(IEnumerable<PostThumbnail> posts, MetaData metadata)> GetPostThumbnailsOfUserAsync(string ownerName, PostParameters postParameters)
        {
            var owner= await _userManager.FindByNameAsync(ownerName);
            if(owner == null)
                throw new UserNotFoundException(ownerName);
            var posts = await _repository.Post.GetPostsAsync(owner.Id, postParameters);
            var postThumbnailDtos = _mapper.Map<IEnumerable<PostThumbnail>>(posts);
            return (postThumbnailDtos, posts.MetaData);
        }

        public async Task<(IEnumerable<PostThumbnail> posts, MetaData metadata)> GetExplorePostsAsync(PostParameters postParameters)
        {
            var posts = await _repository.Post.GetLastestPostsAsync(postParameters);
            var postThumbnailDtos = _mapper.Map<IEnumerable<PostThumbnail>>(posts);
            return (postThumbnailDtos, posts.MetaData);
        }

        public async Task<(IEnumerable<PostDto> posts, MetaData metaData)> GetFeedPostsAsync(ClaimsPrincipal user, PostParameters parameters)
        {
            var userId = _userManager.GetUserId(user);

            var pageFeed = await _repository.FeePostCache.GetFeedPostsAsync(userId, parameters);
            var postIds = pageFeed.Select(p => p.postId);
            var postDtos = new List<PostDto>();
            var postNotHitCacheIds = new List<int>();
            foreach (var postId in postIds)
            {
                var post = await _repository.PostCache.GetPostAsync(postId);
                if (post != null)
                    postDtos.Add(post);
                else
                    postNotHitCacheIds.Add(postId);
            }
            var posts = await _repository.Post.GetPostsAsync(postNotHitCacheIds);

            var likedPostIds = await _repository.Like.FilterLikedPostIdsByUser(userId, postIds);
            var postDtoNotHitCaches = await MapPostToPostDtoIncludeMedia(posts);
            foreach (var post in postDtoNotHitCaches)
            {
                await SetUserNameForTaggedUser(post);
                await _repository.PostCache.CreatePostAsync(post);
                postDtos.Add(post);
            }
            await SetLikedAndSavedForPost(userId, postDtos);
            return (postDtos, pageFeed.MetaData);
        }

        public async Task<(IEnumerable<MediaOfUserDto> mediaItems, MetaData metaData)> GetMediasOfUserAsync(string userName, PostParameters postParameters)
        {
            var user = await _userManager.FindByNameAsync(userName);
            if(user == null)
                throw new UserNotFoundException(userName);

            var medias = await _repository.Media.GetMediaItemsOfUserAsync(user.Id, postParameters);
            return (medias, medias.MetaData);
        }
    }
}