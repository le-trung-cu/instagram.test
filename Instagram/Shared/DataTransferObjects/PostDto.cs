
namespace Shared.DataTransferObjects;
public record PostDto
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
    public bool? Liked { get; set; }
    public bool? Saved { get; set; }
    public int LikeCount { get; set; }
    public int CommentCount { get; set; }

    public IEnumerable<MediaItemDto>? MediaItems { get; set; }

    public virtual IEnumerable<MediaItemDto> GetMediaItems()
    {
        return Enumerable.Empty<MediaItemDto>();
    }
    public virtual IEnumerable<UserTagMediaDto> GetUserTaggedInPost()
    {
        return Enumerable.Empty<UserTagMediaDto>();
    }

    public virtual IEnumerable<string> GetUserIdTaggedInPost()
    {
        return Enumerable.Empty<string>();
    }
}