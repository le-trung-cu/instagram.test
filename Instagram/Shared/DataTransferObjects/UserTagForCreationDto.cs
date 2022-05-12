using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Shared.DataTransferObjects
{
    public abstract record UserTagMediaForCreationDto
    {
        public string UserName { get; init; } = null!;

        [BindNever]
        public string? UserId { get; set; }
    }

    public record UserTagPhotoForCreationDto : UserTagMediaForCreationDto
    {
        public double Top { get; init; }
        public double Left { get; init; }
    }
    public record UserTagVideoForCreationDto : UserTagMediaForCreationDto
    { }
}
