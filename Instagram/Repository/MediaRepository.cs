using Contracts;
using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Shared.DataTransferObjects;
using Shared.RequestFeatures;

namespace Repository
{
    public class MediaRepository : RepositoryBase<MediaItem>, IMediaRepository
    {
        public MediaRepository(RepositoryContext repositoryContext) : base(repositoryContext)
        {
        }

        public async Task<Dictionary<int, IEnumerable<MediaItem>>> GetMediaItemsOfPosts(IEnumerable<int> postIds)
        {
            var medias = await FindByCondition(t => postIds.Contains(t.PostId))
                .Include(t => t.UserTags)
                .ToListAsync();

            var mediasOfPost = medias
                .GroupBy(t => t.PostId)
                .ToDictionary(t => t.Key, t => t.Select(i => i));
            return mediasOfPost;
        }

        public async Task<PagedList<MediaOfUserDto>> GetMediaItemsOfUserAsync(string userId, PostParameters postParameters)
        {
            var medias = await PagedList<MediaOfUserDto>.ToPagedListAsync(
                 RepositoryContext.Set<UserTagMedia>()
                 .Include(t => t.MediaItem!.Post)
                 .Where(t => t.UserId == userId)
                 .OrderByDescending(t => t.Id)
                 .Select(t => new MediaOfUserDto(t.MediaItem!.Id, t.MediaItem.PostId, t.MediaItem.Post!.Slug!, t.MediaItem.Post.PostType, t.MediaItem.Path, t.MediaItem.MediaType))
                 , postParameters.PageNumber, postParameters.PageSize);

            return medias;
        }
    }
}
