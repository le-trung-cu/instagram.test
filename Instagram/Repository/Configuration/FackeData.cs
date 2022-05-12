using Bogus;
using Entities.Models;
using Microsoft.AspNetCore.Identity;

namespace Repository.Configuration
{
    public class FackeData
    {
        public List<User> Users = new List<User>();
        public List<Follow> Follows = new List<Follow>();
        public List<PostPhoto> PostPhotos = new List<PostPhoto>();
        public List<PostCarousel> PostCarousels = new List<PostCarousel>();
        public List<UserTagPhoto> UserTagPhotos = new List<UserTagPhoto>();
        public List<PhotoItem> PhotoItems = new List<PhotoItem>();
        public List<ActivityPostCount> ActivityPostCounts = new List<ActivityPostCount>();
        public List<ActivityCommentCount> ActivityCommentCounts = new List<ActivityCommentCount>();
        public List<ActivityUserDataCount> ActivityUserDataCounts = new List<ActivityUserDataCount>();

        public List<Comment> Comments = new List<Comment>();
        public List<LikeComment> LikeComments = new List<LikeComment>();
        public List<LikePost> LikePosts = new List<LikePost>();

        public const int MAX_COMMENT_IN_POST = 15;
        public const int MIN_COMMENT_IN_POST = 5;
        public const int MAX_LIKE_IN_POST = 10;
        public const int MIN_LIKE_IN_POST = 0;

        public const int MAX_MEDIA_IN_POST = 3;
        public const int MIN_MEDIA_IN_POST = 2;

        public const int MAX_USER_TAG = 2;
        private List<string> _avatars = new List<string>();
        private List<string> _avatarHomes = new List<string>();


        public FackeData()
        {
            for (int i = 1; i <= 10; i++)
            {
                var name = i < 10 ? "cat-0" + i : "cat-" + i;
                _avatars.Add("https://localhost:7299/avatar/" + name + "s150x150.jpg");
                _avatarHomes.Add("https://localhost:7299/avatar/" + name + "s320x320.jpg");
            }
        }

        public string CreateHashPassword(User user)
        {
            var passHash = new PasswordHasher<User>();
            return passHash.HashPassword(user, "Password123");
        }
        public void Init()
        {
            InitUsers(5);
            InitFollow();
            InitPosts(countPostPhoto: 20, countCarousel: 10);
            InitActivityUserDataCount();
            InitActivityPostCount();
            InitActityCommentCount();
        }

        private void InitActivityUserDataCount()
        {
            var userPostPhotoCount = PostPhotos.GroupBy(t => t.OwnerId).ToDictionary(t => t.Key, t => t.Count());
            var userPostCaroselCount = PostCarousels.GroupBy(t => t.OwnerId).ToDictionary(t => t.Key, t => t.Count());
            var userFollowerCount = Follows.GroupBy(t => t.FolloweeId).ToDictionary(t => t.Key, t => t.Count());
            var userFollowingCount = Follows.GroupBy(t => t.FollowerId).ToDictionary(t => t.Key, t => t.Count());

            var activityUserDataCounts = Users.Select(t => new ActivityUserDataCount
            {
                UserId = t.Id,
                Followers = userFollowerCount.GetValueOrDefault(t.Id, 0),
                Following = userFollowingCount.GetValueOrDefault(t.Id, 0),
                Posts = userPostPhotoCount.GetValueOrDefault(t.Id, 0) + userPostCaroselCount.GetValueOrDefault(t.Id, 0),
            });

            ActivityUserDataCounts.AddRange(activityUserDataCounts);
        }

        private void InitActivityPostCount()
        {
            var postCommentCount = Comments.GroupBy(t => t.PostId).ToDictionary(t => t.Key, t => t.Count());
            var postLikeCount = LikePosts.GroupBy(t => t.PostId).ToDictionary(t => t.Key, t => t.Count());

            var postPhotoActivity = PostPhotos.Select(t => new ActivityPostCount
            {
                PostId = t.Id,
                LikeCount = postLikeCount.GetValueOrDefault(t.Id, 0),
                CommentCount = postCommentCount.GetValueOrDefault(t.Id, 0)
            });
            var postCarouselActivity = PostCarousels.Select(t => new ActivityPostCount
            {
                PostId = t.Id,
                LikeCount = postLikeCount.GetValueOrDefault(t.Id, 0),
                CommentCount = postCommentCount.GetValueOrDefault(t.Id, 0)
            });

            ActivityPostCounts.AddRange(postPhotoActivity);
            ActivityPostCounts.AddRange(postCarouselActivity);
        }

        private void InitActityCommentCount()
        {
            var commentLikeCount = LikeComments.GroupBy(t => t.CommentId).ToDictionary(t => t.Key, t => t.Count());
            var commentActivity = Comments.Select(t => new ActivityCommentCount
            {
                CommentId = t.Id,
                LikeCount = commentLikeCount.GetValueOrDefault(t.Id, 0),
                CommentCount = 0
            });

            ActivityCommentCounts.AddRange(commentActivity);
        }
        public void InitUsers(int count)
        {
            var userFake = new Faker<User>()
                .RuleFor(t => t.Id, _ => Guid.NewGuid().ToString())
                .RuleFor(t => t.Email, f => f.Person.Email)
                .RuleFor(t => t.NormalizedEmail, (_, b) => b.Email.ToUpper())
                .RuleFor(t => t.UserName, f => f.Person.UserName)
                .RuleFor(t => t.NormalizedUserName, (_, b) => b.UserName.ToUpper())
                .RuleFor(t => t.PasswordHash, (_, b) => CreateHashPassword(b))
                .RuleFor(t => t.Avatar, f => f.Random.ListItem(_avatars))
                .RuleFor(t => t.AvatarHome, f => f.Random.ListItem(_avatarHomes))
                .RuleFor(t => t.FullName, f => f.Person.FullName);

            var users = userFake.Generate(count);
            Users.AddRange(users);
        }

        public void InitFollow()
        {
            var userIds = Users.Select(t => t.Id).ToList();
            var followFake = new Faker<FollowRecord>()
                .RuleFor(t => t.FollowerId, f => f.Random.ListItem(userIds))
                .RuleFor(t => t.FolloweeId, (f, t) => f.Random.ListItem(userIds));

            var followRecords = followFake.Generate(userIds.Count * (userIds.Count - 1))
                .Where(t => t.FollowerId != t.FolloweeId)
                .Distinct();
            var follows = followRecords.Select(t => new Follow { FollowerId = t.FollowerId, FolloweeId = t.FolloweeId }).ToList();
            Follows.AddRange(follows);
        }

        public void InitPosts(int countPostPhoto, int countCarousel)
        {
            var userTagId = 1;
            var userTagPhotoFake = new Faker<UserTagPhoto>()
                .RuleFor(t => t.Id, _ => userTagId++)
                .RuleFor(t => t.Left, f => f.Random.Double(0.2, 0.8))
                .RuleFor(t => t.Top, f => f.Random.Double(0.2, 0.8))
                .RuleFor(t => t.UserId, f => f.Random.ListItem(Users.Select(u => u.Id).ToList()));

            var mediaItemId = 1;
            var photoItemFake = new Faker<PhotoItem>()
                .RuleFor(t => t.Id, _ => mediaItemId++)
                .RuleFor(t => t.MediaType, "PhotoItem")
                .RuleFor(t => t.Path, f => f.Image.PicsumUrl())
                .RuleFor(t => t.UserTags, (f, t) =>
                {
                    userTagPhotoFake.RuleFor(t2 => t2.MediaItemId, t.Id);
                    var userTags = userTagPhotoFake.GenerateBetween(0, MAX_USER_TAG);
                    UserTagPhotos.AddRange(userTags);
                    return null;
                });
            var commentId = 1;
            var commentFake = new Faker<Comment>()
                .RuleFor(t => t.Id, _ => commentId++)
                .RuleFor(t => t.Content, f => f.Lorem.Sentences(2))
                .RuleFor(t => t.OwnerId, f => f.Random.ListItem(Users.Select(t => t.Id).ToList()))
                .RuleFor(t => t.Likes, (f, t) =>
                {
                    var likes = f.Random.ListItems(
                        Users.Select(u => new LikeComment { CommentId = t.Id, OwnerId = u.Id }).ToList(),
                        Math.Min(5, Users.Count));

                    LikeComments.AddRange(likes);
                    return null;
                });



            var postId = 444_444;
            var postPhotoFake = new Faker<PostPhoto>()
                .RuleFor(t => t.Id, _ => postId++)
                .RuleFor(t => t.OwnerId, f => f.Random.ListItem(Users.Select(t => t.Id).ToList()))
                .RuleFor(t => t.Slug, (f, t) => new Shared.Base53(t.Id).Slug)
                .RuleFor(t => t.Content, f => f.Lorem.Sentence())
                .RuleFor(t => t.EnableComment, _ => true)
                .RuleFor(b => b.PostType, "PostPhoto")
                .RuleFor(t => t.Thumbnail, f => f.Image.PicsumUrl())
                .RuleFor(t => t.Comments, (f, t) =>
                {
                    commentFake.RuleFor(t2 => t2.PostId, t.Id);
                    var comments = commentFake.GenerateBetween(MIN_COMMENT_IN_POST, MAX_COMMENT_IN_POST);
                    Comments.AddRange(comments);
                    return null;
                })
                .RuleFor(t => t.Liskes, (f, t) =>
                {
                    var likes = f.Random.ListItems(Users.Select(u => new LikePost { PostId = t.Id, OwnerId = u.Id }).ToList());
                    LikePosts.AddRange(likes);
                    return null;
                })
                .RuleFor(b => b.PhotoItem, (f, t) =>
                {
                    photoItemFake.RuleFor(t2 => t2.PostId, t.Id);
                    var photoItem = photoItemFake.Generate();
                    PhotoItems.Add(photoItem);
                    return null;
                });
            var postCarouselFake = new Faker<PostCarousel>()
                .RuleFor(t => t.Id, _ => postId++)
                .RuleFor(t => t.OwnerId, f => f.Random.ListItem(Users.Select(t => t.Id).ToList()))
                .RuleFor(t => t.Slug, (f, t) => new Shared.Base53(t.Id).Slug)
                .RuleFor(t => t.Content, f => f.Lorem.Sentence())
                .RuleFor(t => t.EnableComment, _ => true)
                .RuleFor(t => t.PostType, "PostCarousel")
                .RuleFor(t => t.Thumbnail, f => f.Image.PicsumUrl())
                .RuleFor(t => t.Comments, (f, t) =>
                {
                    commentFake.RuleFor(t2 => t2.PostId, t.Id);
                    var comments = commentFake.GenerateBetween(MIN_COMMENT_IN_POST, MAX_COMMENT_IN_POST);
                    Comments.AddRange(comments);
                    return null;
                })
                .RuleFor(t => t.Liskes, (f, t) =>
                {
                    var likes = f.Random.ListItems(Users.Select(u => new LikePost { PostId = t.Id, OwnerId = u.Id }).ToList());
                    LikePosts.AddRange(likes);
                    return null;
                })
                .RuleFor(t => t.MediaItems, (f, t) =>
                {
                    photoItemFake.RuleFor(t => t.PostId, t.Id);
                    var photoItems = photoItemFake.GenerateBetween(MIN_MEDIA_IN_POST, MAX_MEDIA_IN_POST);
                    PhotoItems.AddRange(photoItems);
                    return null;
                });

            var postPhotos = postPhotoFake.Generate(countPostPhoto);
            var postCarousels = postCarouselFake.Generate(countCarousel);

            PostCarousels.AddRange(postCarousels);
            PostPhotos.AddRange(postPhotos);
        }
    }
}

record FollowRecord
{
    public string? FollowerId { get; set; }
    public string? FolloweeId { get; set; }
}