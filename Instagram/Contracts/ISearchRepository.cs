using Shared.DataTransferObjects;

namespace Contracts
{
    public interface ISearchRepository
    {
        Task<IEnumerable<UserForSearchResultDto>> SearchUserByUserName(string username);
    }
}
