using Entities.Models;
using Shared.DataTransferObjects;
using Shared.RequestFeatures;

namespace Contracts
{

    public interface IFollowRepository
    {
        void AddFollow(Follow follow);
        Task<PagedList<UserFollowDto>> GetFolloweesForUser(string userId, PageParameters parameters);
        Task<PagedList<UserFollowDto>> GetFollowersForUser(string userId, PageParameters parameters);
        Task<IEnumerable<UserFollowDto>> GetFollowersForUser(string userId);
        Task<Follow?> GetFollowAsync(string followerId, string followingId);
        Task<IEnumerable<SuggestionFollowDto>> GetSugestionFollowForUser(string userId);
        Task<IEnumerable<string>> FilterFollowingByUser(string userId, IEnumerable<string> followingIds);
        void RemoveFolloweeForUser(Follow follow);
    }

 
}
