namespace Shared.DataTransferObjects
{
    public record MediaOfUserDto(int Id, int PostId, string Slug, string PostType, string Path, string MediaType);
}
