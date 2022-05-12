using Entities.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Repository.Configuration;
public class UserConfiguration : IEntityTypeConfiguration<User>
{
    

    public void Configure(EntityTypeBuilder<User> builder)
    {
        var user1 = new User
        {
            Id = ConstHelper.user1Id,
            UserName = "user1",
            NormalizedUserName = "USER1",
            Email = "user1@gmail.com",
            NormalizedEmail = "USER1@GMAIL.COM",
            PhoneNumber = "XXXXXXXXXXXXX",
            EmailConfirmed = true,
            PhoneNumberConfirmed = true,
            SecurityStamp = new Guid().ToString("D"),
        };

        user1.PasswordHash = PassGenerate(user1);

        var user2 = new User
        {
            Id = ConstHelper.user2Id,
            UserName = "user2",
            NormalizedUserName = "USER2",
            Email = "user2@gmail.com",
            NormalizedEmail = "USER2@GMAIL.COM",
            PhoneNumber = "XXXXXXXXXXXXX",
            EmailConfirmed = true,
            PhoneNumberConfirmed = true,
            SecurityStamp = new Guid().ToString("D"),
        };

        user2.PasswordHash = PassGenerate(user2);

        builder.HasData(user1, user2);
    }

    public string PassGenerate(User user)
    {
        var passHash = new PasswordHasher<User>();
        return passHash.HashPassword(user, "Password123");
    }
}
