namespace Shared.DataTransferObjects
{
    public record UserFollowDto
    {
        public string? UserName { get; set; }
        public string? FullName { get; set; }
        public string? Avatar { get; set; }
        public string? Id { get; set; }
        public string? Status { get; set; }
    }
}
