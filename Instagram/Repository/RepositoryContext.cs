//using Entities.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Entities.Models;
using Entities;
using Repository.Configuration;
using System.Data;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Repository
{
    #region
    //public class TestUser
    //{
    //    public int Id { get; set; }
    //    public string Name { get; set; } = null!;
    //    public IList<TestUserTag>? TestUserTags{ get; set; }
    //}

    //public class TestUserTag
    //{
    //    public int Id { get; set; }
    //    public string Lable { get; set; } = null!;
    //    public int? TestUserId { get; set; }
    //    public TestUser? TestUser { get; set; }
    //}
    //public class TestUserTagComment : TestUserTag
    //{
    //}
    //public class TestUserTagPost : TestUserTag
    //{
    //}

    //public class XXXXRepositoryContext : DbContext
    //{

    //    public XXXXRepositoryContext(DbContextOptions options) : base(options)
    //    { }
    //    public DbSet<TestUser>? TestUsers { get; set; }
    //    public DbSet<TestUserTag>? testUserTags { get; set; }
    //    public DbSet<TestUserTagComment>? testUserTagComments { get;}
    //    public DbSet<TestUserTagPost>? testUserTagPosts { get; set; }

    //    protected override void OnModelCreating(ModelBuilder modelBuilder)
    //    {
    //        base.OnModelCreating(modelBuilder);
    //        modelBuilder.Entity<TestUser>()
    //            .HasMany(u => u.TestUserTags)
    //            .WithOne(t => t.TestUser)
    //            .OnDelete(DeleteBehavior.SetNull);
    //    }

    //}
    #endregion
    public class RepositoryContext : IdentityDbContext<User>
    {
        public RepositoryContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }

        public IDbConnection Connection => Database.GetDbConnection();

        public DbSet<Follow>? Follows { get; set; }
        public DbSet<Post>? Posts { get; set; }
        public DbSet<PostPhoto>? PostPhotos { get; set; }
        public DbSet<PostVideo>? PostVideos { get; set; }
        public DbSet<PostCarousel>? PostCarousels { get; set; }
        public DbSet<Comment>? Commnents { get; set; }
        public DbSet<LikePost>? LikePosts { get; set;}
        public DbSet<LikeComment>? LikeComments { get; set; }
        public DbSet<ActivityPostCount>? ActivityPostCounts { get; set; }
        public DbSet<ActivityCommentCount>? ActivityCommentCounts { get; set; }

        public DbSet<UserTagMedia> UserTagMedias { get; set; } = null!;
        public DbSet<UserTagVideo>? UserTagVideos { get; set; }
        public DbSet<UserTagPhoto>? UserTagPhotos { get; set; }

        public DbSet<MediaItem>? MediaItems { get; set; }
        public DbSet<PhotoItem>? PhotoItems { get; set; }
        public DbSet<VideoItem>? VideoItems { get; set; }
        public DbSet<ActivityUserDataCount>? ActivityUserDataCounts { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<User>()
                .HasMany(t => t.UserFollowees)
                .WithMany(t => t.UserFollowers)
                .UsingEntity<Follow>(
                    t => t.HasOne(f => f.Followee).WithMany(f => f.Followers).HasForeignKey(f => f.FolloweeId),
                    t => t.HasOne(f => f.Follower).WithMany(f => f.Followees).HasForeignKey(f => f.FollowerId),
                    t => t.HasKey(f => new {f.FollowerId, f.FolloweeId}));

            builder.Entity<Follow>()
                .HasOne(t => t.Follower)
                .WithMany(t => t.Followees)
                .HasForeignKey(f => f.FollowerId)
                .OnDelete(DeleteBehavior.ClientCascade);

            builder.Entity<MediaItem>()
                .HasOne(media => media.Post)
                .WithMany()
                .HasForeignKey(media => media.PostId);
            builder.Entity<VideoItem>().HasBaseType<MediaItem>();
            builder.Entity<PhotoItem>().HasBaseType<MediaItem>();

            builder.Entity<Post>()
               .HasOne(p => p.Owner)
               .WithMany(u => u.Posts)
               .HasForeignKey(p => p.OwnerId);
            builder.Entity<Post>().HasDiscriminator(p => p.PostType);
            builder.Entity<PostPhoto>().HasBaseType<Post>();
            builder.Entity<PostVideo>().HasBaseType<Post>();
            builder.Entity<PostCarousel>().HasBaseType<Post>();

            builder.Entity<PostPhoto>()
                .HasOne(p => p.PhotoItem)
                .WithOne()
                .HasForeignKey<PhotoItem>("PostId")
                .OnDelete(DeleteBehavior.ClientCascade);

            builder.Entity<PostVideo>()
               .HasOne(p => p.VideoItem)
               .WithOne()
               .HasForeignKey<VideoItem>("PostId")
               .OnDelete(DeleteBehavior.ClientCascade);

            builder.Entity<PostCarousel>()
               .HasMany(p => p.MediaItems)
               .WithOne()
               .HasForeignKey(media => media.PostId)
               .OnDelete(DeleteBehavior.ClientCascade);


            builder.Entity<UserTagPhoto>().HasBaseType<UserTagMedia>();
            builder.Entity<UserTagVideo>().HasBaseType<UserTagMedia>();

            builder.Entity<UserTagMedia>()
                .HasOne(t => t.MediaItem)
                .WithMany(t => t.UserTags)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Post>()
                .HasMany(t => t.Savers)
                .WithMany(t => t.SavedPosts)
                .UsingEntity<UserSavedPost>(
                    t => t.HasOne(i => i.Owner).WithMany(i => i.UserSavedPosts).HasForeignKey(i => i.OwnerId),
                    t => t.HasOne(i => i.Post).WithMany(i => i.UserSavedPosts).HasForeignKey(i => i.PostId).IsRequired(false),
                    t => t.HasKey(i => new { i.OwnerId, i.PostId  }));

            builder.Entity<LikePost>().HasKey(t => new {t.PostId, t.OwnerId});
            builder.Entity<LikePost>()
                .HasOne(t => t.Owner)
                .WithMany(t => t.LikePosts)
                .OnDelete(DeleteBehavior.ClientCascade);

            builder.Entity<LikeComment>().HasKey(t => new {t.CommentId, t.OwnerId});
            builder.Entity<LikeComment>()
                .HasOne(t => t.Owner)
                .WithMany(t => t.LikeComments)
                .OnDelete(DeleteBehavior.ClientCascade);

            builder.Entity<MediaItem>().HasDiscriminator(p => p.MediaType);
            builder.Entity<MediaItem>()
                .HasIndex(media => media.PostId)
                .IsUnique(false);

            builder.Entity<ActivityUserDataCount>()
                .HasKey(t => t.UserId);
            //builder.Entity<User>()
            //    .HasOne(t => t.ActivityUserDataCount)
            //    .WithOne()
            //    .HasForeignKey<ActivityUserDataCount>(t => t.UserId);


            //var fakeData = new FackeData();
            //fakeData.Init();

            //builder.Entity<User>().HasData(fakeData.Users);
            //builder.Entity<Follow>().HasData(fakeData.Follows);
            //builder.Entity<PostPhoto>().HasData(fakeData.PostPhotos);
            //builder.Entity<PostCarousel>().HasData(fakeData.PostCarousels);
            //builder.Entity<PhotoItem>().HasData(fakeData.PhotoItems);
            //builder.Entity<UserTagPhoto>().HasData(fakeData.UserTagPhotos);
            //builder.Entity<Comment>().HasData(fakeData.Comments);
            //builder.Entity<LikePost>().HasData(fakeData.LikePosts);
            //builder.Entity<LikeComment>().HasData(fakeData.LikeComments);
            //builder.Entity<ActivityPostCount>().HasData(fakeData.ActivityPostCounts);
            //builder.Entity<ActivityCommentCount>().HasData(fakeData.ActivityCommentCounts);
            //builder.Entity<ActivityUserDataCount>().HasData(fakeData.ActivityUserDataCounts);
        }

        //public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        //{
        //    var insertedEntries = ChangeTracker.Entries()
        //        .Where(t => t.State == EntityState.Added)
        //        .Select(t => t.Entity);
        //    var modifiedEntries = ChangeTracker.Entries()
        //        .Where(t => t.State == EntityState.Modified)
        //        .Select(t => t.Entity);

        //    foreach (var entry in insertedEntries)
        //    {
        //        if(entry is ITimestamps timestamps)
        //        {
        //            timestamps.CreatedAt = DateTimeOffset.UtcNow;
        //        }
        //    }
        //    foreach (var entry in modifiedEntries)
        //    {
        //        if(entry is ITimestamps timestamps)
        //        {
        //            timestamps.UpdatedAt = DateTimeOffset.UtcNow;
        //        }
        //    }
        //    return base.SaveChangesAsync(cancellationToken);
        //}
    }

}