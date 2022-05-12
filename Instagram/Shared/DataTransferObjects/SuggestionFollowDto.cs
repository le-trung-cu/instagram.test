namespace Shared.DataTransferObjects
{
    public record SuggestionFollowDto
    {
        public IList<string> FriendNames { get; set; } = new List<string>();
        public int Friends { get; set; }
        public string? UserName { get; set; }
        public string? FullName { get; set; }
        public string? Avatar { get; set; }
        public string? Id { get; set; }
        public string? Subtile => "follow by " + string.Join(", ", FriendNames) + (Friends > FriendNames.Count ? $" and {Friends - FriendNames.Count} others." : "");
    }
}
