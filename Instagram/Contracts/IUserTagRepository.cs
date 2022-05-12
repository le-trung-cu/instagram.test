using Shared.DataTransferObjects;
using Shared.RequestFeatures;

namespace Contracts
{
    public interface IUserTagRepository
    {
        HashSet<int> FilterMediaIdsHasUserTag(IEnumerable<int> mediaIds);
    }
}
