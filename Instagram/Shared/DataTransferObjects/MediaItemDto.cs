namespace Shared.DataTransferObjects
{
    public record MediaItemDto
    {
        public int Id { get; set; }
        public string Path { get; set; } = string.Empty;
        public string MediaType { get; set; } = string.Empty;
        public List<UserTagMediaDto>? UserTags { get; set; }
    }
}
