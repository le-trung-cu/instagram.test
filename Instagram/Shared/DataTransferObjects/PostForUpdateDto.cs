namespace Shared.DataTransferObjects
{
    public record PostForUpdateDto
    {
        public string? Content { get; init; }
        public bool? EnableComment { get; init; }
    }
}
