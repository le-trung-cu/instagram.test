using Shared.DataTransferObjects;

namespace Service.Contracts
{
    public interface INotificationService
    {
        void SendMessage(string userId, NotificationDto notification);
    }
}