using Contracts;
using Newtonsoft.Json;
using Shared.DataTransferObjects;
using StackExchange.Redis;

namespace Repository;

public class PostCacheRepository : IPostCacheRepository
{
    private readonly IConnectionMultiplexer _redis;
    private const string prefix = "PostDto";
    public PostCacheRepository(IConnectionMultiplexer redis)
    {
        _redis = redis;
    }

    public async Task CreatePostAsync(PostDto post)
    {
        var db = _redis.GetDatabase();
        var hashEntrys = new HashEntry[]
        {
            new HashEntry("data", JsonConvert.SerializeObject(post)),
            new HashEntry("likes", post.LikeCount),
            new HashEntry("comments", post.CommentCount),
        };
        await db.HashSetAsync($"{prefix}:{post.Id}", hashEntrys);
    }

    public async Task<PostDto?> GetPostAsync(int postId)
    {
        var db = _redis.GetDatabase();
        var hashPosts = await db.HashGetAllAsync($"{prefix}:{postId}");
        if (hashPosts.Length == 0)
            return null;
        CachePost? cachePost = null;
        int likes = 0, comments = 0;

        foreach (var hashEntry in hashPosts)
        {
            switch (hashEntry.Name)
            {
                case "data":
                    cachePost = System.Text.Json.JsonSerializer.Deserialize<CachePost>(hashEntry.Value);
                    break;
                case "likes":
                    likes = int.Parse(hashEntry.Value);
                    break;
                case "comments":
                    comments = int.Parse(hashEntry.Value);
                    break;
            }
        }
        if (cachePost == null)
            return null;
        PostDto? postDto = cachePost!.ToPostDto() with { LikeCount = likes, CommentCount = comments };
        return postDto;
    }

    public async Task IncrementLikesAsync(int postId)
    {
        var db = _redis.GetDatabase();
        await db.HashIncrementAsync($"{prefix}:{postId}", "likes");
    }

    public async Task DecrementLikesAsync(int postId)
    {
        var db = _redis.GetDatabase();
        await db.HashDecrementAsync($"{prefix}:{postId}", "likes");
    }

    public async Task IncrementCommentsAsync(int postId)
    {
        var db = _redis.GetDatabase();
        await db.HashIncrementAsync($"{prefix}:{postId}", "comments");
    }
    public async Task DecremenCommentsAsync(int postId)
    {
        var db = _redis.GetDatabase();
        await db.HashDecrementAsync($"{prefix}:{postId}", "comments");
    }
}

record CacheUserTag(int Id, string UserId, string UserName, double Top, double Left)
{
    public UserTagMediaDto ToUserTagMedia(string mediaType)
    {
        return mediaType switch
        {
            "PhotoItem" => ToUserTagPhotoDto(),
            "ViedeoItem" => ToUserTagVideoDto(),
            _ => throw new NotImplementedException(),
        };
    }
    public UserTagPhotoDto ToUserTagPhotoDto()
    {
        return new UserTagPhotoDto { Id = Id, UserId = UserId, UserName = UserName, Top = Top, Left = Left };
    }

    public UserTagVideoDto ToUserTagVideoDto()
    {
        return new UserTagVideoDto { Id = Id, UserId = UserId, UserName = UserName };
    }
}

record CacheMediaItem(int Id, string Path, string MediaType, IEnumerable<CacheUserTag> UserTags)
{
    public MediaItemDto ToMediaItemDto()
    {
        var media = new MediaItemDto { Id = Id, Path = Path, MediaType = MediaType };
        var userTags = new List<UserTagMediaDto>();

        userTags.AddRange(UserTags.Select(t => t.ToUserTagMedia(MediaType)));
        media.UserTags = userTags;
        return media;
    }
}
record CachePost
{
    public int Id { get; init; }
    public string? Slug { get; set; }
    public string? Content { get; init; }
    public bool EnableComment { get; init; }
    public DateTimeOffset CreatedAt { get; init; }
    public DateTimeOffset UpdatedAt { get; init; }
    public string? OwnerId { get; init; }
    public string? UserName { get; init; }
    public string? Title { get; set; }
    public string? Avatar { get; init; }
    public string? PostType { get; init; }

    public IEnumerable<CacheMediaItem>? MediaItems { get; set; }

    public PostDto ToPostDto()
    {
        return new PostDto
        {
            Id = Id,
            Slug = Slug,
            Content = Content,
            EnableComment = EnableComment,
            CreatedAt = CreatedAt,
            UpdatedAt = UpdatedAt,
            OwnerId = OwnerId,
            UserName = UserName,
            Title = Title,
            Avatar = Avatar,
            PostType = PostType,
            MediaItems = MediaItems?.Select(t => t.ToMediaItemDto()),
        };
    }
}