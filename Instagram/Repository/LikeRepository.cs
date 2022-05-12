using Contracts;
using Entities.Models;
using Microsoft.EntityFrameworkCore;

namespace Repository;
public class LikeRepository : ILikeRepository
{
    private readonly Lazy<LikeCommentRepository> _likeCommentRepository;
    private readonly Lazy<LikePostRepository> _likePostRepository;

    public LikeRepository(RepositoryContext repositoryContext)
    {
        _likeCommentRepository = new Lazy<LikeCommentRepository>(() => new LikeCommentRepository(repositoryContext));
        _likePostRepository = new Lazy<LikePostRepository>(() => new LikePostRepository(repositoryContext));
    }

    
    public void CreateLikeComment(LikeComment like)
    {
        LikeComment.Create(like);
    }

    public void CreateLikePost(LikePost like)
    {
        LikePost.Create(like);
    }

    public void DeleteLikeComment(LikeComment like)
    {
        LikeComment.Delete(like);
    }

    public void DeleteLikePost(LikePost like)
    {
        LikePost.Delete(like);
    }

    public async Task<IEnumerable<int>> FilterLikedPostIdsByUser(string userId, IEnumerable<int> postIds)
    {
        return await LikePost.FindByCondition(t => t.OwnerId == userId && postIds.Contains(t.PostId))
            .Select(t => t.PostId)
            .ToListAsync();
    }

    public async Task<IEnumerable<int>> FilterLikedCommentIdsByUser(string userId, IEnumerable<int> commentIds)
    {
        return await LikeComment.FindByCondition(t => t.OwnerId == userId && commentIds.Contains(t.CommentId))
            .Select(t => t.CommentId)
            .ToListAsync();
    }

    public async Task<bool> CheckPostLiked(string userId, int postId)
    {
        return await LikePost.FindByCondition( t => t.OwnerId == userId && t.PostId == postId ).AnyAsync();
    }


    private LikeCommentRepository LikeComment => _likeCommentRepository.Value;
    private LikePostRepository LikePost => _likePostRepository.Value;
}

internal class LikeCommentRepository : RepositoryBase<LikeComment>
{
    public LikeCommentRepository(RepositoryContext repositoryContext) : base(repositoryContext)
    {
    }
}

internal class LikePostRepository : RepositoryBase<LikePost>
{
    public LikePostRepository(RepositoryContext repositoryContext) : base(repositoryContext)
    {
    }
}


