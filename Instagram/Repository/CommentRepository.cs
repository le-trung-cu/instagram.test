using Contracts;
using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Shared.RequestFeatures;

namespace Repository
{
    public class CommentRepository : RepositoryBase<Comment>, ICommentRepository
    {
        public CommentRepository(RepositoryContext repositoryContext) : base(repositoryContext)
        {
        }

        public void CreateComment(Comment comment)
        {
            comment.ActivityCommentCount = comment.ActivityCommentCount ?? new ActivityCommentCount();
            Create(comment);
        }

        public void DeleteComment(Comment comment)
        {
            Delete(comment);
        }

        public async Task<Comment?> GetCommentAsync(int commentId)
        {
            return await FindByCondition(c => c.Id == commentId)
                .Include(c => c.Owner)
                .Include(c => c.ActivityCommentCount)
                .FirstOrDefaultAsync();
        }

        public async Task<PagedList<Comment>> GetCommentsOfPostAsync(int postId, CommentParameters commentParameters, string? authId)
        {
            var query = FindByCondition(c => c.PostId == postId)
                .Include(c => c.ActivityCommentCount).AsQueryable()
                .Include(c => c.Owner)
                .AsQueryable();

            if (commentParameters.SelectedShow == RequestParameters.SelectedShows.Latest)
            {
                query = query.OrderByDescending(t => t.CreatedAt);
            }
            else if (commentParameters.SelectedShow == RequestParameters.SelectedShows.Popular)
            {
                query = query.OrderBy(t => t.ActivityCommentCount!.CommentCount)
                    .ThenBy(t => t.ActivityCommentCount!.LikeCount);
            }
            else if (commentParameters.SelectedShow == RequestParameters.SelectedShows.Suitable)
            {
                query = (from f in RepositoryContext.Follows
                         join c in RepositoryContext.Commnents!
                         on f.FolloweeId equals c.OwnerId
                         where f.FolloweeId == authId && c.PostId == postId
                         join u in RepositoryContext.Users
                         on c.OwnerId equals u.Id
                         select new Comment { Id = c.Id, OwnerId = c.OwnerId, Content = c.Content, Owner = c.Owner })
                         .AsQueryable();
            }
            return await PagedList<Comment>.ToPagedListAsync(query, commentParameters.PageNumber, commentParameters.PageSize);
        }
    }
}
