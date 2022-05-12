namespace Entities.Models;
public abstract class MediaItem
{
    public int Id { get; set; }
    public string Path { get; set; } = null!;
    public string MediaType { get; set; } = null!;
    public int PostId { get; set; }
    public Post? Post { get; set; }
    public List<UserTagMedia>? UserTags { get; set; }
}

public class PhotoItem : MediaItem
{
    //public new ICollection<UserTagPhoto>? UserTags { get; set; }
}
public class VideoItem : MediaItem
{
    //public new ICollection<UserTagVideo>? UserTags { get; set; }
}