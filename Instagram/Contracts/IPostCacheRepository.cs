using Shared.DataTransferObjects;

namespace Contracts
{
    public interface IPostCacheRepository
    {
        Task CreatePostAsync(PostDto post);
        Task DecremenCommentsAsync(int postId);
        Task DecrementLikesAsync(int postId);
        Task<PostDto?> GetPostAsync(int postId);
        Task IncrementCommentsAsync(int postId);
        Task IncrementLikesAsync(int postId);
    }
}
