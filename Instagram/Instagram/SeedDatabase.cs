using Bogus;
using Entities.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Repository;

namespace Instagram
{
    public class SeedDatabase
    {
        private const int USER_COUNT = 100;
        private const int MIN_FOLLOWING = 0;
        private const int MAX_FOLLOWING = 50;
        private const int MIN_POST_USER = 20;
        private const int MAX_POST_USER = 50;

        public const int MIN_COMMENT_IN_POST = 0;
        public const int MAX_COMMENT_IN_POST = 50;
        public const int MIN_LIKE_IN_POST = 0;
        public const int MAX_LIKE_IN_POST = 50;

        public const int MAX_MEDIA_IN_POST = 6;
        public const int MIN_MEDIA_IN_POST = 2;

        public const int MAX_USER_TAG = 2;

        private string[] _avatars = new string[10];
        private string[] _avatarHomes = new string[10];

        private DbContextOptionsBuilder<RepositoryContext> _dbOptionsBuilder;
        public SeedDatabase(IConfiguration configuration, RepositoryContext repositoryContext)
        {
            string connectionString = configuration.GetConnectionString("sqlConnection");
            _dbOptionsBuilder = new DbContextOptionsBuilder<RepositoryContext>();
            _dbOptionsBuilder.UseSqlServer(connectionString);

        }

        public void Seed()
        {
            using (var db = new RepositoryContext(_dbOptionsBuilder.Options))
            {
                if (db.Users.Any())
                {
                    return;
                }

                for (int i = 1; i <= 10; i++)
                {
                    var name = i < 10 ? "cat-0" + i : "cat-" + i;
                    _avatars[i - 1] = "https://localhost:7299/avatar/" + name + "s150x150.jpg";
                    _avatarHomes[i - 1] = "https://localhost:7299/avatar/" + name + "s320x320.jpg";
                }
                var userIds = CreateUsers();
                CreateFollows(userIds);
                var posts = CreatePosts(userIds);
                foreach (var post in posts)
                {
                    CreateComments(post, userIds);
                    CreateLikes(post, userIds);
                }

                db.Database.ExecuteSqlRaw(
                    @"INSERT INTO ActivityUserDataCounts(UserId, Followers, Following, Posts) 
                    SELECT Id, 0, 0, 0 FROM AspNetUsers");
                db.Database.ExecuteSqlRaw(
                    @"UPDATE ActivityUserDataCounts
                    SET 
                    Posts = (SELECT COUNT(*) FROM Posts WHERE OwnerId = UserId),
                    Followers = (SELECT COUNT(FollowerId) FROM Follows WHERE FolloweeId = UserId),
                    Following = (SELECT COUNT(FolloweeId) FROM Follows WHERE FollowerId = UserId)
                    ");


            }
        }

        private string[] CreateUsers()
        {
            var userFake = new Faker<User>()
                .RuleFor(t => t.Id, _ => Guid.NewGuid().ToString())
                .RuleFor(t => t.Email, f => f.Person.Email)
                .RuleFor(t => t.NormalizedEmail, (_, b) => b.Email.ToUpper())
                .RuleFor(t => t.UserName, f => f.Person.UserName)
                .RuleFor(t => t.NormalizedUserName, (_, b) => b.UserName.ToUpper())
                .RuleFor(t => t.Avatar, f => f.Random.ListItem(_avatars))
                .RuleFor(t => t.AvatarHome, f => f.Random.ListItem(_avatarHomes))
                .RuleFor(t => t.FullName, f => f.Person.FullName);

            var users = userFake.Generate(USER_COUNT).DistinctBy(t => t.UserName);
            foreach (var range1 in users.Chunk(2000))
            {
                using (var db = new RepositoryContext(_dbOptionsBuilder.Options))
                {
                    foreach (var range2 in range1.Chunk(500))
                    {
                        foreach (var user in range2)
                        {
                            user.PasswordHash = CreateHashPassword(user);
                            db.Users.Add(user);
                        }
                        db.SaveChanges();
                        db.ChangeTracker.Clear();
                    }
                }
            }
            return users.Select(x => x.Id).ToArray();
        }

        private void CreateFollows(string[] userIds)
        {
            var faker = new Faker();

            foreach (var currentUser in userIds)
            {
                var followingCount = faker.Random.Int(MIN_FOLLOWING, MAX_FOLLOWING);
                var followingIds = faker.Random.ArrayElements(userIds, followingCount).Where(t => t != currentUser).Distinct();
                foreach (var range1 in followingIds.Chunk(2000))
                {
                    using (var db = new RepositoryContext(_dbOptionsBuilder.Options))
                    {
                        foreach (var range2 in range1.Chunk(500))
                        {
                            foreach (var followingId in range2)
                            {
                                var follow = new Follow { FollowerId = currentUser, FolloweeId = followingId };
                                db.Follows.Add(follow);
                            }
                            db.SaveChanges();
                            db.ChangeTracker.Clear();
                        }
                    }
                }
            }
        }

        private IEnumerable<(int Id, DateTimeOffset CreateAt)> CreatePosts(string[] userIds)
        {
            var userTagePhotoFake = new Faker<UserTagPhoto>()
                .RuleFor(t => t.UserId, f => f.Random.ArrayElement(userIds))
                .RuleFor(t => t.Top, f => f.Random.Double(0.2, 0.8))
                .RuleFor(t => t.Left, f => f.Random.Double(0.2, 0.8));

            var photoItemFake = new Faker<PhotoItem>()
                .RuleFor(t => t.Path, f => f.Image.PicsumUrl())
                .RuleFor(t => t.MediaType, _ => "PhotoItem");

            var postId = 444_444;
            var postPhotoFake = new Faker<PostPhoto>()
                .RuleFor(t => t.Id, _ => postId++)
                .RuleFor(t => t.Slug, (f, t) => new Shared.Base53(t.Id).Slug)
                .RuleFor(t => t.OwnerId, f => f.Random.ArrayElement(userIds))
                .RuleFor(t => t.Content, f => f.Lorem.Sentence())
                .RuleFor(t => t.EnableComment, _ => true)
                .RuleFor(b => b.PostType, "PostPhoto")
                .RuleFor(t => t.CreatedAt, f => f.Date.Past(2))
                .RuleFor(t => t.Thumbnail, f => f.Image.PicsumUrl())
                .RuleFor(t => t.PhotoItem, _ =>
                {
                    var photoItem = photoItemFake.Generate();
                    photoItem.UserTags = new List<UserTagMedia>();
                    photoItem.UserTags.AddRange(userTagePhotoFake.Generate(2));
                    return photoItem;
                });

            var postCarouselFake = new Faker<PostCarousel>()
                .RuleFor(t => t.Id, _ => postId++)
                .RuleFor(t => t.Slug, (f, t) => new Shared.Base53(t.Id).Slug)
                .RuleFor(t => t.OwnerId, f => f.Random.ArrayElement(userIds))
                .RuleFor(t => t.Content, f => f.Lorem.Sentence())
                .RuleFor(t => t.EnableComment, _ => true)
                .RuleFor(t => t.PostType, "PostCarousel")
                .RuleFor(t => t.CreatedAt, f => f.Date.Past(2))
                .RuleFor(t => t.Thumbnail, f => f.Image.PicsumUrl())
                .RuleFor(t => t.MediaItems, f =>
                {
                    var mediaItems = new List<MediaItem>();
                    var photoItems = photoItemFake.GenerateBetween(MIN_MEDIA_IN_POST, MAX_MEDIA_IN_POST);
                    foreach (var photoItem in photoItems)
                    {
                        photoItem.UserTags = new List<UserTagMedia>();
                        if (f.Random.Bool())
                            photoItem.UserTags.AddRange(userTagePhotoFake.GenerateBetween(0, 2));
                        mediaItems.Add(photoItem);
                    }
                    return mediaItems;
                });

            var faker = new Faker();
            var result = new List<(int, DateTimeOffset)>();
            foreach (var currentUser in userIds)
            {
                var postCount = faker.Random.Int(MIN_POST_USER, MAX_POST_USER);
                foreach (var range1 in Enumerable.Range(0, postCount).Chunk(100))
                {
                    using (var db = new RepositoryContext(_dbOptionsBuilder.Options))
                    {
                        db.Database.OpenConnection();
                        db.Database.ExecuteSqlRaw("SET IDENTITY_INSERT [dbo].[Posts] ON");
                        foreach (var range2 in range1)
                        {
                            Post post;
                            if (faker.Random.Bool())
                            {
                                post = postPhotoFake.Generate();
                            }
                            else
                            {
                                post = postCarouselFake.Generate();
                            }
                            db.Posts.Add(post);
                            result.Add((post.Id, post.CreatedAt));
                        }
                        db.SaveChanges();
                        db.Database.ExecuteSqlRaw("SET IDENTITY_INSERT [dbo].[Posts] OFF");
                        db.Database.CloseConnection();
                    }
                }
            }

            return result;
        }

        private void CreateComments((int Id, DateTimeOffset CreatedAt) post, string[] userIds)
        {
            var commentFaker = new Faker<Comment>()
                .RuleFor(t => t.PostId, _ => post.Id)
                .RuleFor(t => t.Content, f => f.Lorem.Sentences(2))
                .RuleFor(t => t.OwnerId, f => f.Random.ArrayElement(userIds))
                .RuleFor(t => t.CreatedAt, f => f.Date.BetweenOffset(post.CreatedAt, DateTime.Now));

            var comments = commentFaker.GenerateBetween(MIN_COMMENT_IN_POST, MAX_COMMENT_IN_POST);
            foreach (var range1 in comments.Chunk(100))
            {
                using (var db = new RepositoryContext(_dbOptionsBuilder.Options))
                {
                    foreach (var comment in range1)
                    {
                        comment.ActivityCommentCount = new ActivityCommentCount();
                        db.Commnents.Add(comment);
                    }
                    db.SaveChanges();
                }
            }

            using (var db = new RepositoryContext(_dbOptionsBuilder.Options))
            {
                var activity = db.Set<ActivityPostCount>().FirstOrDefault(t => t.PostId == post.Id);
                if (activity == null)
                {
                    db.Set<ActivityPostCount>().Add(new ActivityPostCount
                    {
                        PostId = post.Id,
                        CommentCount = comments.Count()
                    });
                }
                else
                {
                    activity.CommentCount = comments.Count();
                }
                db.SaveChanges();
            }
        }

        private void CreateLikes((int Id, DateTimeOffset CreatedAt) post, string[] userIds)
        {

            var likeFaker = new Faker<LikePost>()
                .RuleFor(t => t.PostId, _ => post.Id)
                .RuleFor(t => t.CreatedAt, f => f.Date.BetweenOffset(post.CreatedAt, DateTime.Now));
            var faker = new Faker();
            var likeCount = faker.Random.Int(MIN_LIKE_IN_POST, MAX_LIKE_IN_POST);
            var userLikes = faker.Random.ArrayElements(userIds, likeCount).Distinct().ToArray();

            var likes = likeFaker.Generate(userLikes.Length);
            var index = 0;
            foreach (var range1 in likes.Chunk(100))
            {
                using (var db = new RepositoryContext(_dbOptionsBuilder.Options))
                {
                    foreach (var like in range1)
                    {
                        like.OwnerId = userLikes[index++];
                        db.LikePosts!.Add(like);
                    }
                    db.SaveChanges();
                }
            }
            using (var db = new RepositoryContext(_dbOptionsBuilder.Options))
            {
                var activity = db.Set<ActivityPostCount>().FirstOrDefault(t => t.PostId == post.Id);
                if (activity == null)
                {
                    db.ActivityPostCounts.Add(new ActivityPostCount { PostId = post.Id, CommentCount = likes.Count() });
                }
                else
                {
                    activity.LikeCount = likes.Count();
                }
                db.SaveChanges();
            }
        }

        public string CreateHashPassword(User user)
        {
            var passHash = new PasswordHasher<User>();
            return passHash.HashPassword(user, "Password123");
        }
    }
}
