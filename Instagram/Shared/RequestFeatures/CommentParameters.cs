namespace Shared.RequestFeatures
{
    public record CommentParameters : RequestParameters
    {
        public SelectedShows? SelectedShow { get; set; }
    }
}
