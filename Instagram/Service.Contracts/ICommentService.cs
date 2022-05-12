using Entities.Models;
using Shared.DataTransferObjects;
using Shared.RequestFeatures;
using System.Security.Claims;

namespace Service.Contracts
{
    public interface ICommentService
    {
        Task<(IEnumerable<CommentDto> comments, MetaData metaData)> GetCommentsForPostAsync(ClaimsPrincipal user, int postId, CommentParameters commentParameters);
        Task<CommentDto> GetCommentAsync(int commentId);
        Task<CommentDto> CreateCommentAsync(ClaimsPrincipal user, CommentForCreationDto commentForCreation);
        Task DeleteCommentAsync(ClaimsPrincipal user, int commentId);
    }
}
