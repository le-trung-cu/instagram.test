namespace Shared.DataTransferObjects
{
    public record CommentDto
    {
        public int Id { get; set; }
        public string? Content { get; set; }
        public string? OwnerId { get; set; }
        public string? UserName { get; set; }
        public string? Avatar { get; set; }
        public string? FullName { get; set; }
        public int PostId { get; set; }
        public int CommentCount { get; set; }
        public int LikeCount { get; set; }
        public int? ParentId { get; set; }
        public bool Liked { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset UpdatedAt { get; set; }
    }
}