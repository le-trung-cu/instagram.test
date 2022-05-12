using Entities.Models;
using Shared.RequestFeatures;

namespace Contracts
{
    public interface IFeedPostCacheRepository
    {
        Task CreateFeedPostAsync(string userId, FeedPost feedPost );
        Task CreateRangeFeedPostsAsync(string userId, FeedPost[] feedPosts);
        Task<PagedList<FeedPost>> GetFeedPostsAsync(string userId, PostParameters? parameters = null);
        Task RemoveUserFeedPostsOfOwner(string userId, string onwerId);
    }
}
