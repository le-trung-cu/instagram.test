using System.Text.Json.Serialization;

namespace Shared.DataTransferObjects
{
    public record CommentForCreationDto
    {
        [JsonIgnore]
        public int PostId { get; set; }
        public string? Content { get; init; }
        public int? ParentId { get; init; }
    }
}
