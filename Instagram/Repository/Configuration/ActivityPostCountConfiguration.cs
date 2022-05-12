using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Repository.Configuration
{
    public class ActivityPostCountConfiguration : IEntityTypeConfiguration<ActivityPostCount>
    {
        public void Configure(EntityTypeBuilder<ActivityPostCount> builder)
        {
            var activityPost1Count = new ActivityPostCount
            {
                PostId = 1,
                CommentCount = 3,
                LikeCount = 0
            };
            var activityPost2Count = new ActivityPostCount { PostId = 2 };
            var activityPost3Count = new ActivityPostCount { PostId = 3 };
            builder.HasData(activityPost1Count, activityPost2Count, activityPost3Count);
        }
    }
}
