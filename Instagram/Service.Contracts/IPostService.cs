using Shared.DataTransferObjects;
using Shared.RequestFeatures;
using System.Security.Claims;

namespace Service.Contracts;

public interface IPostService
{
    Task<(IEnumerable<PostThumbnail> posts, MetaData metadata)> GetExplorePostsAsync(PostParameters postParameters);
    Task<(IEnumerable<PostDto> posts, MetaData metaData)> GetPostsAsync(string ownerName, PostParameters postParameters, ClaimsPrincipal? user = null);
    Task<(IEnumerable<PostThumbnail> posts, MetaData metadata)> GetPostThumbnailsOfUserAsync(string ownerName, PostParameters postParameters);
    Task<(IEnumerable<PostThumbnail> posts, MetaData metaData)> GetSavedPostsOfUser(ClaimsPrincipal user, PostParameters postParameters);
    Task<(IEnumerable<MediaOfUserDto> mediaItems, MetaData metaData)> GetMediasOfUserAsync(string userName, PostParameters postParameters);
    Task<PostDto> GetPostAsync(int postId, ClaimsPrincipal? user);
    Task<PostDto> CreatePostAsync(PostBaseForCreationDto postForCreation, ClaimsPrincipal user);
    Task UpdatePostAsync(int postId, PostForUpdateDto postForUpdate);
    Task DeletePostAsync(int postId, ClaimsPrincipal user);
    Task SavePost(ClaimsPrincipal user, int id);
    Task DeleteSavedPost(ClaimsPrincipal user, int id);
    Task<(IEnumerable<PostDto> posts, MetaData metaData)> GetFeedPostsAsync(ClaimsPrincipal user, PostParameters parameters);
}
