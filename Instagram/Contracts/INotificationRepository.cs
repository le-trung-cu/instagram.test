using Shared.DataTransferObjects;
using Shared.RequestFeatures;

namespace Contracts
{

    public interface INotificationRepository
    {
        Task CreateNotifiCation(string userId, NotificationDto notification);
        Task<PagedList<NotificationDto>> GetNotifiCations(string userId, PageParameters parameters);
    }
}
