using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Repository.Configuration
{
    public class ActivityCommentCountConfiguration : IEntityTypeConfiguration<ActivityCommentCount>
    {
        public void Configure(EntityTypeBuilder<ActivityCommentCount> builder)
        {
            var activityComment1Count = new ActivityCommentCount
            {
                CommentId = 1,
                CommentCount = 1,
                LikeCount = 0
            };
            var activityComment2Count = new ActivityCommentCount { CommentId = 2 };
            var activityComment3Count = new ActivityCommentCount {CommentId = 3 };
            builder.HasData(activityComment1Count, activityComment2Count, activityComment3Count);
        }
    }
}
