using Entities.Models;

namespace Contracts
{
    public interface ILikeRepository
    {
        public void CreateLikePost(LikePost like);
        public void DeleteLikePost(LikePost like);

        public void CreateLikeComment(LikeComment like);
        public void DeleteLikeComment(LikeComment like);
        Task<IEnumerable<int>> FilterLikedPostIdsByUser(string userId, IEnumerable<int> postIds);
        Task<IEnumerable<int>> FilterLikedCommentIdsByUser(string userId, IEnumerable<int> commentIds);
        Task<bool> CheckPostLiked(string userId, int postId);
    }
}
