using Entities.Models;
using Shared.RequestFeatures;

namespace Contracts
{
    public interface ICommentRepository
    {
        Task<PagedList<Comment>> GetCommentsOfPostAsync(int postId, CommentParameters commentParameters, string? authId = null);
        Task<Comment?> GetCommentAsync(int commentId);
        void CreateComment(Comment comment);
        void DeleteComment(Comment comment);
    }
}
