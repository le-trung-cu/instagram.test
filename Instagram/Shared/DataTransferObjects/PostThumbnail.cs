namespace Shared.DataTransferObjects
{

    public record PostThumbnail(int Id, string Slug, string PostType, int LikeCount, int CommentCount)
    {
        public string? Thumbnail { get; set; }
    };
}
