using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Models;
public class Comment : ITimestamps
{
    public int Id { get; set; }

    public string Content { get; set; } = null!;

    [ForeignKey(nameof(User))]
    public string? OwnerId { get; set; } = null!;
    public User? Owner { get; set; }
    public int PostId { get; set; }
    public Post? Post { get; set; }
    public ICollection<LikeComment>? Likes { get; set; }
    public ActivityCommentCount? ActivityCommentCount { get; set; }
    public int? ParentId { get; set; }
    public virtual Comment? Parent { get; set; }
    public ICollection<Comment>? Children { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
}

