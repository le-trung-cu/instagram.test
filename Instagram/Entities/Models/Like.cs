using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Models;
public abstract class Like : ITimestamps
{

    [ForeignKey(nameof(User))]
    public string OwnerId { get; set; } = null!;
    public User? Owner { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
}

public class LikePost : Like
{
    public int PostId { get; set; }
    public Post? Post { get; set; }
}

public class LikeComment : Like
{
    public int CommentId { get; set; }
    public Comment? Comment { get; set; }
}