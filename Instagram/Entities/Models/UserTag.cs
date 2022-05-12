namespace Entities.Models;

public abstract class UserTagMedia
{
    public int Id { get; set; }
    public string? UserId { get; set; }
    public User? User { get; set; }
    public int MediaItemId { get; set; }
    public MediaItem? MediaItem { get; set; }
}

public class UserTagVideo : UserTagMedia
{
    public new VideoItem? MediaItem { get; set; }
}
public class UserTagPhoto : UserTagMedia
{
    public new PhotoItem? MediaItem { get; set; }
    public double Top { get; set; }
    public double Left { get; set; }
}