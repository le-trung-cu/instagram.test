using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Models;
public class Post : ITimestamps
{
    public static readonly Post DeletedPost = new Post { Content = "Post have been deleted." };
    public int Id { get; set; }
    public string? Slug { get; set; }
    public string Content { get; set; } = null!;
    public bool EnableComment { get; set; } = true;
    public string PostType { get; set; } = null!;

    [ForeignKey(nameof(User))]
    public string OwnerId { get; set; } = null!;
    public User? Owner { get; set; }
    public ICollection<Comment>? Comments { get; set; }
    public ICollection<LikePost>? Liskes { get; set; }
    public ActivityPostCount? ActivityPostCount { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }

    public ICollection<User>? Savers { get; set; }
    public List<UserSavedPost>? UserSavedPosts { get; set; }

    public string? Thumbnail { get; set; }
    public virtual IEnumerable<string> GetUserIdTaggedInPost()
    {
        return Enumerable.Empty<string>();
    }

    public virtual IEnumerable<UserTagMedia> GetUserTagInPost()
    {
        return Enumerable.Empty<UserTagMedia>();
    }
}

public class PostPhoto : Post
{
    public PhotoItem? PhotoItem { get; set; }

    public override IEnumerable<UserTagMedia> GetUserTagInPost()
    {
        if (PhotoItem != null && PhotoItem.UserTags != null)
        {
            return PhotoItem.UserTags.ToList();
        }
        return Enumerable.Empty<UserTagMedia>();
    }
    public override IEnumerable<string> GetUserIdTaggedInPost()
    {
        if (PhotoItem != null && PhotoItem.UserTags != null)
        {
            return PhotoItem.UserTags.Select(t => t.UserId!).Distinct().ToList();
        }
        return Enumerable.Empty<string>();
    }
}

public class PostVideo : Post
{
    public VideoItem? VideoItem { get; set; }

    public override IEnumerable<UserTagMedia> GetUserTagInPost()
    {
        if (VideoItem != null && VideoItem.UserTags != null)
        {
            return VideoItem.UserTags.ToList();
        }
        return Enumerable.Empty<UserTagMedia>();
    }

    public override IEnumerable<string> GetUserIdTaggedInPost()
    {
        if (VideoItem != null && VideoItem.UserTags != null)
        {
            return VideoItem.UserTags.Select(t => t.UserId!).Distinct().ToList();
        }
        return Enumerable.Empty<string>();
    }
}

public class PostCarousel : Post
{
    public ICollection<MediaItem>? MediaItems { get; set; }

    public override IEnumerable<UserTagMedia> GetUserTagInPost()
    {
        var listTag = new List<UserTagMedia>();
        if (MediaItems != null)
            foreach (var media in MediaItems)
            {
                if (media.UserTags != null)
                    listTag.AddRange(media.UserTags);
            }
        return listTag;
    }

    public override IEnumerable<string> GetUserIdTaggedInPost()
    {
        var listId = new HashSet<string>();
        if (MediaItems != null)
            foreach (var media in MediaItems)
            {
                if (media.UserTags != null)
                    foreach (var tag in media.UserTags)
                    {
                        listId.Add(tag.UserId!);
                    }
            }

        return listId.ToList();
    }
}