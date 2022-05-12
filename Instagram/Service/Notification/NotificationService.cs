using Contracts;
using Microsoft.AspNetCore.SignalR;
using Service.Contracts;
using Service.HubCofig;
using Shared.DataTransferObjects;

namespace Service.Notification;
public class NotificationService : INotificationService
{
    private readonly IHubContext<NotificationHub, INotificationHubClient> _notifiCation;

    public NotificationService(IHubContext<NotificationHub, INotificationHubClient> notifiCation)
    {
        _notifiCation = notifiCation;
    }



    public async void SendNotificationNewPosts(string userId, int postCount)
    {
        //await _notifiCation.Clients.Group(userId).ReceiveMessage(new NotificationNewPost { Count = postCount });
    }

    public async void SendNotificationNewPosts(List<string> userIds, int postCount)
    {
        //await _notifiCation.Clients.Groups(userIds).ReceiveMessage(new NotificationNewPost { Count = postCount });
    }

    public async void SendNotificationLikePost(NotificationDto notification)
    {
       // await _notifiCation.Clients.Groups($"PostGroup:{notification.Slug}").ReceiveMessage(notification);
    }

    public async Task SendNotificationLikePost(string userId, string userName, bool liked, int postId)
    {
        //try
        //{
        //    var notification = new NotificationLikePost(userId, userName, Liked: liked, Slug: new Shared.Base53(postId).Slug, postId);
        //    var post = await _repository.Post.GetPostAsync(postId);
        //    var postOwnerId = post!.OwnerId;
        //    await _notifiCation.Clients.Group(postOwnerId).ReceiveMessage(notification);
        //}
        //catch (Exception)
        //{
        //}
    }

    public async void SendMessage(string userId, NotificationDto notification)
    {
        await _notifiCation.Clients.Group(userId).ReceiveMessage(notification);
    }
}

