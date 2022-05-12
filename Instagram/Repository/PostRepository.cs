using Contracts;
using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Shared.RequestFeatures;

namespace Repository;

public class PostRepository : RepositoryBase<Post>, IPostRepository
{
    public PostRepository(RepositoryContext repositoryContext) : base(repositoryContext)
    {   
    }

    public void CreatePost(Post post)
    {
        post.ActivityPostCount = post.ActivityPostCount ?? new ActivityPostCount();
        Create(post);
    }

    public void DeletedPost(Post post)
    {
        Delete(post);
    }

    public async Task<IEnumerable<Post>> GetLastestPostOfUserAsync(string ownerId, int count)
    {
        var result = await FindByCondition(p => p.OwnerId == ownerId)
            .OrderByDescending(p => p.CreatedAt)
            .Take(count)
            .ToListAsync();
        return result;
    }

    public async Task<PagedList<Post>> GetLastestPostsAsync(PostParameters parameters)
    {
        var posts = await PagedList<Post>.ToPagedListAsync(FindAll().Include(t => t.Owner)
           .Include(p => p.ActivityPostCount)
           .OrderByDescending(t => t.CreatedAt),
           parameters.PageNumber, parameters.PageSize);

        return posts;
    }


    public async Task<Post?> GetPostAsync(int postId, bool trackChanges)
    {
        var post = await FindByCondition(p => p.Id == postId, trackChanges)
            .Include(p => p.Owner)
            .Include(p => p.ActivityPostCount)
            .FirstOrDefaultAsync();
        return post;
    }


    public async Task<PagedList<Post>> GetPostsAsync(string ownerId, PostParameters postParameters)
    {
        var posts = await PagedList<Post>.ToPagedListAsync(
            FindByCondition(p => p.OwnerId == ownerId)
            .Include(p => p.Owner)
            .Include(p => p.ActivityPostCount).OrderByDescending(t => t.CreatedAt),
            postParameters.PageNumber, postParameters.PageSize);

        return posts;
    }


    public async Task<IEnumerable<Post>> GetPostsAsync(IEnumerable<int> ids)
    {
        var posts = await FindByCondition(p => ids.Contains(p.Id))
            .Include(p => p.Owner)
            .Include(p => p.ActivityPostCount)
            .ToListAsync();
        return posts;
    }
}

