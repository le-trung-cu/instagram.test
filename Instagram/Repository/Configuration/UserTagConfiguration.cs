using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Repository.Configuration
{
    internal class UserTagConfiguration : IEntityTypeConfiguration<UserTagPhoto>
    {
        public void Configure(EntityTypeBuilder<UserTagPhoto> builder)
        {
            var user1TagPhoto1 = new UserTagPhoto
            {
                Id = 1,
                MediaItemId = 1,
                Top = 0.25,
                Left = 0.25,
                UserId = ConstHelper.user1Id,
            };
            var user2TagPhoto1 = new UserTagPhoto
            {
                Id = 2,
                MediaItemId = 1,
                Left = 0.75,
                Top = 0.75,
                UserId = ConstHelper.user2Id,
            };
            builder.HasData(user1TagPhoto1, user2TagPhoto1);

        }
    }
}