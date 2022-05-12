namespace Contracts;
public interface IRepositoryManager
{
    IPostRepository Post { get; }
    IFollowRepository Follow { get; }
    ILikeRepository Like { get; }
    ICommentRepository Comment { get; }
    IActivityCountRepository ActivityCount { get; }
    IMediaRepository Media { get; }
    IActivityUserDataCountRepository ActivityUserData { get; }
    IUserTagRepository UserTag { get; }
    IPostCacheRepository PostCache { get; }
    IFeedPostCacheRepository FeePostCache { get; }
    IUserSavePostRepository SavePost { get; }
    INotificationRepository NotificationRepository { get; }
    Task SaveAsync();
}

