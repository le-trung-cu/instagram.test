using Microsoft.AspNetCore.Identity;

namespace Entities.Models
{
    public class User : IdentityUser
    {
        public string? Avatar { get; set; }
        public string? AvatarHome { get; set; }
        public string? FullName { get; set; }
        public ICollection<User>? UserFollowees { get; set; }
        public ICollection<User>? UserFollowers { get; set; }
        public ICollection<Follow>? Followers { get; set; }
        public ICollection<Follow>? Followees { get; set; }
        public ICollection<Comment>? Comments { get; set; }
        public ICollection<LikePost>? LikePosts { get; set; }
        public ICollection<LikeComment>? LikeComments { get; set; }
        public ICollection<UserTagMedia>? UserTagMedias { get; set; }
        public ICollection<Post>? Posts { get; set; }
        public ICollection<Post>? SavedPosts { get; set; }
        public List<UserSavedPost>? UserSavedPosts { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime RefreshTokenExpiryTime { get; set; }

        public ActivityUserDataCount? ActivityUserDataCount { get; set; }
    }
}
