namespace Shared.RequestFeatures
{
    public record PostParameters : RequestParameters
    {
        public bool Thumbnail { get; set; } = false;
    }
}
