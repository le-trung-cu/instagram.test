using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Repository.Configuration;
public class PostConfiguration : IEntityTypeConfiguration<Post>
{
    public void Configure(EntityTypeBuilder<Post> builder)
    {
        var post1OfUser1 = new Post
        {
            Id = 1,
            OwnerId = ConstHelper.user1Id,
            Content = "Post 1 content",
            EnableComment = true,
        };
        var post2OfUser2 = new Post
        {
            Id = 2,
            OwnerId = ConstHelper.user1Id,
            Content = "Post 2 content",
            EnableComment = true,
        };

        builder.HasData(post1OfUser1, post2OfUser2);
    }
}

public class PostPhotoConfiguration : IEntityTypeConfiguration<PostPhoto>
{
    public void Configure(EntityTypeBuilder<PostPhoto> builder)
    {
        var post3OfUser1 = new PostPhoto
        {
            Id = 3,
            OwnerId = ConstHelper.user1Id,
            Content = "Post 3 content",
            EnableComment = true,
        };

        builder.HasData(post3OfUser1);
    }
}
