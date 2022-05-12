namespace Shared.DataTransferObjects;
public class NotificationDto
{
    public NotifiCationTypes Type { get; set; }
    public string? Message { get; set; }
    public string? UserName { get; set; }
    public string? UserId { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
}

public enum NotifiCationTypes
{
    UserLikePost,
    UserLikeComment,
    UserCommentInPost,
    FollowingNewPost,
}
