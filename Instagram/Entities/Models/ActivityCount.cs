using System.ComponentModel.DataAnnotations;

namespace Entities.Models;

public abstract class ActivityCount
{
    public int CommentCount { get; set; }
    public int LikeCount { get; set; }
}
public class ActivityPostCount : ActivityCount
{
    [Key]
    public int PostId { get; set; }
    public Post? Post { get; set; }
}

public class ActivityCommentCount : ActivityCount
{
    [Key]
    public int CommentId { get; set; }
    public Comment? Comment { get; set; }
}