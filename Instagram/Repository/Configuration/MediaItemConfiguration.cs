using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Repository.Configuration;
public class PhotoItemConfiguration : IEntityTypeConfiguration<PhotoItem>
{
    public void Configure(EntityTypeBuilder<PhotoItem> builder)
    {
        var photo1OfPost3 = new PhotoItem
        {
            Id = 1,
            PostId = 3,
            Path = "photo1.jpg",
        };
        builder.HasData(photo1OfPost3);
    }
}
