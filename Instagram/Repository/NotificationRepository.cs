using Contracts;
using Shared.DataTransferObjects;
using Shared.RequestFeatures;
using StackExchange.Redis;
using System.Text.Json;

namespace Repository;


public class NotificationRepository : INotificationRepository
{
    private Lazy<IDatabase> _database;
    private IDatabase _db => _database.Value;
    private const int range = 10;


    private const string prefix = "NotifiCation";
    public NotificationRepository(IConnectionMultiplexer redis)
    {
        _database = new Lazy<IDatabase>(() => redis.GetDatabase());

    }

    public async Task CreateNotifiCation(string userId, NotificationDto notification)
    {
        var key = $"{prefix}:{userId}";
        await _db.ListLeftPushAsync(key, JsonSerializer.Serialize(notification));
        await _db.ListTrimAsync(key, 0, range);
    }

    public async Task<PagedList<NotificationDto>> GetNotifiCations(string userId, PageParameters parameters)
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
        var count = (int)await _db.ListLengthAsync(key);
        var notifications = Array.ConvertAll(values, t => JsonSerializer.Deserialize<NotificationDto>(t)!).ToList();
        var pageFeed = new PagedList<NotificationDto>(notifications, count, parameters!.PageNumber, parameters.PageSize);
        return pageFeed;
    }
}

