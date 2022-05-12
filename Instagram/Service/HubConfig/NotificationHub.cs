using Microsoft.AspNetCore.SignalR;
using Shared.DataTransferObjects;
using System.Security.Claims;

namespace Service.HubCofig
{
    public interface INotificationHubClient
    {
        Task ReceiveMessage(string message);
        Task ReceiveMessage(NotificationDto notification);
    }

    public class NotificationHub : Hub<INotificationHubClient>
    {
        public async void RegisterReceiveNotification(string userId)
        {
            if (Context.User.Claims.Any(t => ClaimTypes.NameIdentifier == t.Type && t.Value == userId))
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, userId);
                await Clients.Caller.ReceiveMessage($"RegisterReceiveNotification user with id: {userId}");
            }
            else await Clients.Caller.ReceiveMessage($"RegisterReceiveNotification fail UnAuthenticated User");
        }
        
        public async void AddUserToGroup(string groupName)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
            await Clients.Caller.ReceiveMessage($"Cuttent User was added to group {groupName}");
        }

        public async Task RemoveUserFromGroup(string groupName)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);
            await Clients.Caller.ReceiveMessage($"Current User was removed from group {groupName}");
        }
    }
}
