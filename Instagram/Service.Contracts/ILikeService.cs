using System.Security.Claims;

namespace Service.Contracts
{
    public interface ILikeService
    {
        Task LikePostAsync(ClaimsPrincipal user, int postId);
        Task UnLikePost(ClaimsPrincipal user, int postId);
        Task LikeCommentAsync(ClaimsPrincipal user, int commentId);
        Task UnLikeCommentAsync(ClaimsPrincipal user, int commentId);
    }
}
