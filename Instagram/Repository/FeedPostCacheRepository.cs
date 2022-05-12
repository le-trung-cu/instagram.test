using Contracts;
using Entities.Models;
using Shared.RequestFeatures;
using StackExchange.Redis;
using System.Text.Json;

namespace Repository;

public class FeedPostCacheRepository : IFeedPostCacheRepository
{
    private static string prefix = "FeedPost";
    private const int range = 10;
    private Lazy<IDatabase> _database;
    private IDatabase _db => _database.Value;

    public FeedPostCacheRepository(IConnectionMultiplexer redis)
    {
        _database = new Lazy<IDatabase>(() => redis.GetDatabase());
    }

    public async Task CreateFeedPostAsync(string userId, FeedPost feedPost)
    {
        await _db.ListLeftPushAsync($"{prefix}:{userId}", JsonSerializer.Serialize(feedPost));
        await _db.ListTrimAsync($"{prefix}:{userId}", 0, range);
    }

    public async Task CreateRangeFeedPostsAsync(string userId, FeedPost[] feedPosts)
    {
        var values = Array.ConvertAll(feedPosts, t => (RedisValue) JsonSerializer.Serialize(t)!);
        await _db.ListLeftPushAsync ($"{prefix}:{userId}", values);
    }

    public async Task<PagedList<FeedPost>> GetFeedPostsAsync(string userId, PostParameters? parameters)
    {
        var key = $"{prefix}:{userId}";
        var start = 0;
        var stop = -1;
        if (parameters != null)
        {
            start = (parameters.PageNumber - 1) * parameters.PageSize;
            stop = parameters.PageNumber * parameters.PageSize - 1;
        }
        var values = await _db.ListRangeAsync(key, start, stop);
        var count = (int) await _db.ListLengthAsync(key);
        var feedPost = Array.ConvertAll(values, t => JsonSerializer.Deserialize<FeedPost>(t)!).ToList();
        var pageFeed = new PagedList<FeedPost>(feedPost, count, parameters!.PageNumber, parameters.PageSize);
        return pageFeed;
    }

    public async Task RemoveUserFeedPostsOfOwner(string userId, string onwerId)
    {
        var feedPosts = await GetFeedPostsAsync(userId, new PostParameters { PageNumber = 1, PageSize = range});
        if(feedPosts.Any(t => t.OwnerId == onwerId))
        {
            var newFeedPosts = feedPosts.Where(t => t.OwnerId != onwerId).ToArray();
            await _db.KeyDeleteAsync($"{prefix}:{userId}");
            await CreateRangeFeedPostsAsync(userId, newFeedPosts);
        }
    }
}
