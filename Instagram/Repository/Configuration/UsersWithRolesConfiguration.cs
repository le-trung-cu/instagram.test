using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Repository.Configuration;
public class UsersWithRolesConfiguration : IEntityTypeConfiguration<IdentityUserRole<string>>
{
    private const string user1Id = ConstHelper.user1Id;
    private const string adminRoleId = ConstHelper.adminRoleId;

    public void Configure(EntityTypeBuilder<IdentityUserRole<string>> builder)
    {
        IdentityUserRole<string> iur = new IdentityUserRole<string>
        {
            RoleId = adminRoleId,
            UserId = user1Id
        };

        builder.HasData(iur);
    }
}