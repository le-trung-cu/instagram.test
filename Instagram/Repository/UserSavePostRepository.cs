using Contracts;
using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Shared.DataTransferObjects;
using Shared.RequestFeatures;

namespace Repository
{
    public class UserSavePostRepository : RepositoryBase<UserSavedPost>, IUserSavePostRepository
    {
        public UserSavePostRepository(RepositoryContext repositoryContext) : base(repositoryContext)
        {
        }

        public void DeleteSavedPost(UserSavedPost savePost)
        {
            Delete(savePost);
        }

        public async Task<IEnumerable<int>> FilterSavedPostIdsByUser(string userId, IEnumerable<int> postIds)
        {
            return await FindByCondition(t => t.OwnerId == userId && postIds.Contains(t.PostId))
            .Select(t => t.PostId)
            .ToListAsync();
        }

        public void SavePost(UserSavedPost savedPost)
        {
            Create(savedPost);
        }

        public async Task<PagedList<UserSavedPost>> GetSavedPostOfUser(string userId, PostParameters parameters)
        {
            var posts = await PagedList<UserSavedPost>.ToPagedListAsync(
                FindByCondition(t => t.OwnerId == userId)
                .Include(t => t.Post)
                .ThenInclude(t => t.ActivityPostCount)
                .OrderByDescending(t => t.SavedDate),
                parameters.PageNumber, parameters.PageSize);
            return posts;
        }

        public async Task<bool> IsSavedByUser(string userId, int postId)
        {
            return await FindByCondition(t => t.OwnerId == userId && t.PostId == postId).AnyAsync();
        }
    }


}
