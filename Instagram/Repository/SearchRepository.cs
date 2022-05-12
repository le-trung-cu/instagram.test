using Contracts;
using Microsoft.EntityFrameworkCore;
using Shared.DataTransferObjects;

namespace Repository
{
   

    public class SearchRepository : ISearchRepository
    {
        private RepositoryContext _repository;

        public SearchRepository(RepositoryContext repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<UserForSearchResultDto>> SearchUserByUserName(string username)
        {
            return await _repository.Users
                .Where(t => t.UserName.Contains(username))
                .Take(10)
                .Select(t => new UserForSearchResultDto { Id = t.Id, UserName = t.UserName, Avatar = t.Avatar, Title = t.FullName })
                .ToListAsync();
        }
    }
}
