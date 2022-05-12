namespace Entities.Models
{
    public class ActivityUserDataCount
    {
        public string UserId { get; set; } = null!;
        public User? User { get; set; }
        public int Followers { get; set; }
        public int Following { get; set; }
        public int Posts { get; set; }
    }
}
