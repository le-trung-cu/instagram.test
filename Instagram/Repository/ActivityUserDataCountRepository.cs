using Contracts;
using Entities.Models;
using Microsoft.EntityFrameworkCore;

namespace Repository
{
    public class ActivityUserDataCountRepository : RepositoryBase<ActivityUserDataCount>, IActivityUserDataCountRepository
    {
        public ActivityUserDataCountRepository(RepositoryContext repositoryContext) : base(repositoryContext)
        {
        }

        public async Task IncreasePostCount(string userId)
        {
            await RepositoryContext.Database.ExecuteSqlRawAsync(
                "UPDATE ActivityUserDataCounts" +
                " SET Posts = Posts + 1" +
                " WHERE UserId = {0}", userId);
        }

        public async Task DescreasePostCount(string userId)
        {
            await RepositoryContext.Database.ExecuteSqlRawAsync(
                "UPDATE ActivityUserDataCounts" +
                " SET Posts = Posts - 1" +
                " WHERE UserId = {0}", userId);
        }

        public async Task IncreaseFollowerCount(string userId)
        {
            await RepositoryContext.Database.ExecuteSqlRawAsync(
               "UPDATE ActivityUserDataCounts" +
               " SET Followers = Followers + 1" +
               " WHERE UserId = {0}", userId);
        }
        public async Task DescreaseFollowerCount(string userId)
        {
            await RepositoryContext.Database.ExecuteSqlRawAsync(
               "UPDATE ActivityUserDataCounts" +
               " SET Followers = Followers - 1" +
               " WHERE UserId = {0}", userId);
        }

        public async Task IncreaseFollowingCount(string userId)
        {
            await RepositoryContext.Database.ExecuteSqlRawAsync(
               "UPDATE ActivityUserDataCounts" +
               " SET Following = Following + 1" +
               " WHERE UserId = {0}", userId);
        }

        public async Task DescreaseFollowingCount(string userId)
        {
            await RepositoryContext.Database.ExecuteSqlRawAsync(
               "UPDATE ActivityUserDataCounts" +
               " SET Following = Following - 1" +
               " WHERE UserId = {0}", userId);
        }
    }
}
