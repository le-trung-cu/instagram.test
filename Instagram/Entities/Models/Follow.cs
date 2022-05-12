namespace Entities.Models;
public class Follow : ITimestamps
{
    public DateTime FollowDate { get; set; }
    public string FollowerId { get; set; } = null!;
    public string FolloweeId { get; set; } = null!;
    public User? Follower { get; set; }
    public User? Followee { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
}
