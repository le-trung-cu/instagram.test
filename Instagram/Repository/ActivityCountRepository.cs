using Contracts;
using Entities.Models;
using Microsoft.EntityFrameworkCore;

namespace Repository
{
    public class ActivityCountRepository : RepositoryBase<ActivityCount>, IActivityCountRepository
    {
        public ActivityCountRepository(RepositoryContext repositoryContext) : base(repositoryContext)
        {
        }

        public async Task IncreaseLikeCountOfPostAsync(int postId)
        {
            await RepositoryContext.Database.ExecuteSqlRawAsync(
                "UPDATE ActivityPostCounts" +
                " SET LikeCount = LikeCount + 1" +
                " WHERE PostId = {0}", postId);
        }
        public async Task DescreaseLikeCoutOfPostAsync(int postId)
        {
            await RepositoryContext.Database.ExecuteSqlRawAsync(
                "UPDATE ActivityPostCounts" +
                " SET LikeCount = LikeCount - 1" +
                " WHERE PostId = {0} AND LikeCount > 0", postId);
        }

        public async Task IncrementCommentCountOfPostAsync(int postId)
        {
            await RepositoryContext.Database.ExecuteSqlRawAsync(
                "UPDATE ActivityPostCounts" +
                " SET CommentCount = CommentCount + 1" +
                " WHERE PostId = {0}", postId);
        }

        public async Task DescrementCommentCoutOfPost(int postId)
        {
            await RepositoryContext.Database.ExecuteSqlRawAsync(
                "UPDATE ActivityPostCounts" +
                " SET CommentCount = CommentCount - 1" +
                " WHERE PostId = {0} AND CommentCount > 0", postId);
        }

        public async Task IncreaseLikeCountOfCommentAsync(int commentId)
        {
            await RepositoryContext.Database.ExecuteSqlRawAsync(
                "UPDATE ActivityCommentCounts" +
                " SET LikeCount = LikeCount + 1" +
                " WHERE CommentId = {0}", commentId);
        }
        public async Task DescreaseLikeCoutOfCommentAsync(int commentId)
        {
            await RepositoryContext.Database.ExecuteSqlRawAsync(
                "UPDATE ActivityCommentCounts" +
                " SET LikeCount = LikeCount - 1" +
                " WHERE CommentId = {0} AND LikeCount > 0", commentId);
        }
        public async Task IncrementCommentCountOfCommentAsync(int commentId)
        {
            await RepositoryContext.Database.ExecuteSqlRawAsync(
                "UPDATE ActivityCommentCounts" +
                " SET CommentCount = CommentCount + 1" +
                " WHERE CommentId = {0}", commentId);
        }

        

        public async Task DescrementCommentCoutOfCommentAsync(int commentId)
        {
            await RepositoryContext.Database.ExecuteSqlRawAsync(
                "UPDATE ActivityCommentCounts" +
                " SET CommentCount = CommentCount - 1" +
                " WHERE CommentId = {0} AND CommentCount > 0", commentId);
        }
    }
}
