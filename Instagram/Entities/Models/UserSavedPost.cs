namespace Entities.Models
{
    public class UserSavedPost
    {
        public string OwnerId { get; set; } = null!;
        public User? Owner { get; set; }
        public int PostId { get; set; }
        public Post? Post { get; set; }
        public DateTime SavedDate { get; set; } = DateTime.Now;
    }
}
