using Contracts;
using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Shared.DataTransferObjects;
using Shared.RequestFeatures;

namespace Repository
{
    public class UserTagRepository : RepositoryBase<UserTagMedia>, IUserTagRepository
    {
        public UserTagRepository(RepositoryContext repositoryContext) : base(repositoryContext)
        {
        }

        public HashSet<int> FilterMediaIdsHasUserTag(IEnumerable<int> mediaIds)
        {
            return FindByCondition(t => mediaIds.Contains(t.MediaItemId)).Select(t => t.MediaItemId).ToHashSet();
        }
    }
}
