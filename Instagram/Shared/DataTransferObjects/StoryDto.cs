namespace Shared.DataTransferObjects
{
    public record StoryDto
    {
        public string? UserName { get; set; }
        public string? UserId { get; set; }
        public string? Avata { get; set; }
        public int PostId { get; set; }
    }
}
