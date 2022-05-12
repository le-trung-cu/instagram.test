namespace Shared.DataTransferObjects
{
    public record UserDto
    {
        public string Id {get; set;}
        public string? Avatar { get; set; }
        public string? AvatarHome { get; set; }
        public string? UserName { get; set; }
        public string? Title { get; set; }
        public int Followers { get; set; }
        public int Following { get; set; }
        public int Posts { get; set; }
    }
}
