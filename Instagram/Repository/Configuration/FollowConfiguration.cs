using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Repository.Configuration
{
    internal class FollowConfiguration : IEntityTypeConfiguration<Follow>
    {
        public void Configure(EntityTypeBuilder<Follow> builder)
        {
            var user2FollowUser1 = new Follow
            {
                FollowDate = DateTime.Now.AddDays(-10),
                FollowerId = ConstHelper.user2Id,
                FolloweeId = ConstHelper.user1Id,
            };

            builder.HasData(user2FollowUser1);
        }
    }
}
