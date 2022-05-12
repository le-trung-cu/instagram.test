using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Configuration
{
    public class CommentConfiguration : IEntityTypeConfiguration<Comment>
    {
        public void Configure(EntityTypeBuilder<Comment> builder)
        {

            var user2comment1OfPost1 = new Comment
            {
                Id = 1,
                PostId = 1,
                Content = "User 2 comment",
                OwnerId = ConstHelper.user2Id,
                CreatedAt = DateTime.Now.AddDays(-1),
            };
            var user1comment2OfPost1 = new Comment
            {
                Id = 2,
                PostId = 1,
                OwnerId = ConstHelper.user1Id,
                Content = "User 1 comment",
                CreatedAt = DateTime.Now,
            };
            var user1AnswerComment1_comment3 = new Comment
            {
                Id = 3,
                PostId = 1,
                OwnerId = ConstHelper.user1Id,
                ParentId = 1,
                Content = "User 1 answer comment 1",
                CreatedAt = DateTime.Now,
            };

            builder.HasData(user2comment1OfPost1, user1comment2OfPost1, user1AnswerComment1_comment3);
        }
    }
}
