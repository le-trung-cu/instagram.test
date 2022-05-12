namespace Shared.DataTransferObjects
{
    public abstract record UserTagMediaDto
    {
        public int Id { get; set; }
        public string? UserId { get; set; }
        public string? UserName { get; set; }
    }

    public record UserTagPhotoDto : UserTagMediaDto
    {
        public double Top { get; init; }
        public double Left { get; init; }
    }
    public record UserTagVideoDto : UserTagMediaDto
    { }

}
