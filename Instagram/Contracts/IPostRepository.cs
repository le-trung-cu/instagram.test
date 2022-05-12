using Entities.Models;
using Shared.RequestFeatures;

namespace Contracts;

public interface IPostRepository
{
    Task<PagedList<Post>> GetPostsAsync(string ownerId, PostParameters postParameters);
    Task<Post?> GetPostAsync(int postId, bool trackChanges = false);
    Task<IEnumerable<Post>> GetLastestPostOfUserAsync(string ownerId, int count = 3);
    Task<PagedList<Post>> GetLastestPostsAsync(PostParameters parameters);
    void CreatePost(Post post);
    Task<IEnumerable<Post>> GetPostsAsync(IEnumerable<int> ids);
    void DeletedPost(Post post);
}

