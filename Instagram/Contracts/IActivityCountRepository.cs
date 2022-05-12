using Entities.Models;

namespace Contracts
{
    public interface IActivityCountRepository
    {
        Task DescrementCommentCoutOfCommentAsync(int commentId);
        Task DescrementCommentCoutOfPost(int postId);
        Task DescreaseLikeCoutOfCommentAsync(int commentId);
        Task DescreaseLikeCoutOfPostAsync(int postId);
        Task IncrementCommentCountOfCommentAsync(int commentId);
        Task IncrementCommentCountOfPostAsync(int postId);
        Task IncreaseLikeCountOfCommentAsync(int commentId);
        Task IncreaseLikeCountOfPostAsync(int postId);
    }
}