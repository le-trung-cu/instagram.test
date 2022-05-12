using Entities.Models;
using Shared.DataTransferObjects;
using Shared.RequestFeatures;

namespace Contracts
{
    public interface IUserSavePostRepository
    {
        void SavePost(UserSavedPost savedPost);
        void DeleteSavedPost(UserSavedPost savePost);
        Task<IEnumerable<int>> FilterSavedPostIdsByUser(string userId, IEnumerable<int> postIds);
        Task<PagedList<UserSavedPost>> GetSavedPostOfUser(string userId, PostParameters parameters);
        Task<bool> IsSavedByUser(string userId, int postId);
    }
}
