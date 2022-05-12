using AutoMapper;
using Entities.Models;
using Shared.DataTransferObjects;

namespace Instagram;
public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<UserForRegistrationDto, User>();
        // For Get Post
        CreateMap<Post, PostDto>()
            .ForMember(t => t.UserName, t => t.MapFrom(a => a.Owner!.UserName))
            .ForMember(t => t.Avatar, t => t.MapFrom(a => a.Owner!.Avatar))
            .ForMember(t => t.Title, t => t.MapFrom(a => a.Owner!.FullName))
            .ForMember(t => t.LikeCount, t => t.MapFrom(a => a.ActivityPostCount!.LikeCount))
            .ForMember(t => t.CommentCount, t => t.MapFrom(a => a.ActivityPostCount!.CommentCount))
            .ForMember(t => t.MediaItems, t => t.Ignore());

        CreateMap<Post, PostThumbnail>()
            .ForCtorParam(nameof(PostThumbnail.LikeCount), t => t.MapFrom(a => a.ActivityPostCount!.LikeCount))
            .ForCtorParam(nameof(PostThumbnail.CommentCount), t => t.MapFrom(a => a.ActivityPostCount!.CommentCount));

        CreateMap<UserTagMedia, UserTagMediaDto>()
            .Include<UserTagPhoto, UserTagPhotoDto>()
            .Include<UserTagVideo, UserTagVideoDto>();
        CreateMap<UserTagPhoto, UserTagPhotoDto>();
        CreateMap<UserTagVideo, UserTagVideoDto>();

        CreateMap<MediaItem, MediaItemDto>()
            .Include<PhotoItem, MediaItemDto>()
            .Include<VideoItem, MediaItemDto>();
        CreateMap<PhotoItem, MediaItemDto>();
        CreateMap<VideoItem, MediaItemDto>();

        // For Create Post
        CreateMap<PostBaseForCreationDto, Post>()
            .Include<PostForCreationDto, Post>()
            .Include<PostPhotoForCreationDto, PostPhoto>()
            .Include<PostVideoForCreationDto, PostVideo>()
            .Include<PostCarouselForCreationDto, PostCarousel>();
        CreateMap<PostForCreationDto, Post>();
        CreateMap<PostPhotoForCreationDto, PostPhoto>();
        CreateMap<PostVideoForCreationDto, PostVideo>();
        CreateMap<PostCarouselForCreationDto, PostCarousel>()
            .ForMember(t => t.MediaItems,
                t => t.MapFrom(a => new List<MediaItemForCreationDto>()
                .Concat(a.VideoItems ?? Enumerable.Empty<MediaItemForCreationDto>())
                .Concat(a.PhotoItems ?? Enumerable.Empty<MediaItemForCreationDto>())));


        CreateMap<MediaItemForCreationDto, MediaItem>()
            .Include<PhotoItemForCreationDto, PhotoItem>()
            .Include<VideoItemForCreationDto, VideoItem>();
        CreateMap<PhotoItemForCreationDto, PhotoItem>();
        CreateMap<VideoItemForCreationDto, VideoItem>();

        CreateMap<UserTagMediaForCreationDto, UserTagMedia>()
            .Include<UserTagPhotoForCreationDto, UserTagPhoto>()
            .Include<UserTagVideoForCreationDto, UserTagVideo>();
        CreateMap<UserTagPhotoForCreationDto, UserTagPhoto>();
        CreateMap<UserTagVideoForCreationDto, UserTagVideo>();

        // For Get Comment
        CreateMap<Comment, CommentDto>()
            .ForMember(t => t.Avatar, t => t.MapFrom(a => a.Owner!.Avatar))
            .ForMember(t => t.UserName, t => t.MapFrom(a => a.Owner!.UserName))
            .ForMember(t => t.FullName, t => t.MapFrom(a => a.Owner!.FullName))
            .ForMember(t => t.CommentCount, t => t.MapFrom(a => a.Post!.ActivityPostCount!.CommentCount))
            .ForMember(t => t.LikeCount, t => t.MapFrom(a => a.Post!.ActivityPostCount!.LikeCount));

        // For Create Comment
        CreateMap<CommentForCreationDto, Comment>();

        CreateMap<User, UserForSearchResultDto>();
        CreateMap<User, UserDto>()
            .ForMember(t => t.Posts, t => t.MapFrom(a => a.ActivityUserDataCount!.Posts))
            .ForMember(t => t.Followers, t => t.MapFrom(a => a.ActivityUserDataCount!.Followers))
            .ForMember(t => t.Following, t => t.MapFrom(a => a.ActivityUserDataCount!.Following));

    }
}

