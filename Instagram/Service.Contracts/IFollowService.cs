using Shared.DataTransferObjects;
using Shared.RequestFeatures;
using System.Security.Claims;

namespace Service.Contracts
{
    public interface IFollowService
    {
        Task CreateFollowAsync(ClaimsPrincipal follower, string username);
        Task DeleteFollowAsync(ClaimsPrincipal follower, string username);
        Task<(IEnumerable<SuggestionFollowDto> items, MetaData meta)> GetSugestionFollowForUser(ClaimsPrincipal follower, PageParameters parameters);
        Task<(IEnumerable<UserFollowDto> items, MetaData meta)> GetFollowers(ClaimsPrincipal user, string userName, PageParameters parameters);
        Task<(IEnumerable<UserFollowDto> items, MetaData meta)> GetFollowing(ClaimsPrincipal user, string userName, PageParameters parameters);
    }
}