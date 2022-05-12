using Entities.Models;
using Shared.DataTransferObjects;
using Shared.RequestFeatures;

namespace Contracts
{
    public interface IMediaRepository
    {
        Task<PagedList<MediaOfUserDto>> GetMediaItemsOfUserAsync(string userId, PostParameters postParameters);
        Task<Dictionary<int, IEnumerable<MediaItem>>> GetMediaItemsOfPosts(IEnumerable<int> postIds);
    }
}
