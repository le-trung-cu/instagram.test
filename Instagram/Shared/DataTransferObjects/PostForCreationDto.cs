namespace Shared.DataTransferObjects
{
    public abstract record PostBaseForCreationDto
    {
        public string? Content { get; init; }
        public bool EnableComment { get; set; } = true;
        public string? PostType { get; init; }
        public abstract IEnumerable<string> GetUserNamesInMediaItem();

        public abstract void SetUserIdOfUserTags(Dictionary<string, string> userTaggeds);
    }

    public record PostForCreationDto : PostBaseForCreationDto
    {
        public override IEnumerable<string> GetUserNamesInMediaItem()
        {
            return Enumerable.Empty<string>();
        }

        public override void SetUserIdOfUserTags(Dictionary<string, string> userTaggeds)
        {
        }
    }

    public record PostPhotoForCreationDto : PostBaseForCreationDto
    {
        public PhotoItemForCreationDto? PhotoItem { get; set; }

        public override IEnumerable<string> GetUserNamesInMediaItem()
        {
            return PhotoItem!.GetUserNamesInMediaItem();
        }

        public override void SetUserIdOfUserTags(Dictionary<string, string> userTaggeds)
        {
            PhotoItem!.SetUserIdOfUserTags(userTaggeds);
        }
    }

    public record PostVideoForCreationDto : PostBaseForCreationDto
    {
        public VideoItemForCreationDto? VideoItem { get; set; }

        public override IEnumerable<string> GetUserNamesInMediaItem()
        {
            return VideoItem!.GetUserNamesInMediaItem();
        }


        public override void SetUserIdOfUserTags(Dictionary<string, string> userTaggeds)
        {
            VideoItem!.SetUserIdOfUserTags(userTaggeds);
        }
    }

    public record PostCarouselForCreationDto : PostBaseForCreationDto
    {
        public IEnumerable<PhotoItemForCreationDto>? PhotoItems { get; set; }
        public IEnumerable<VideoItemForCreationDto>? VideoItems { get; set; }

        public override IEnumerable<string> GetUserNamesInMediaItem()
        {
            var result = new List<string>();
            if (PhotoItems != null)
                foreach (var mediaItem in PhotoItems)
                {
                    var userNames = mediaItem.GetUserNamesInMediaItem();
                    result.AddRange(userNames);
                }
            if (VideoItems != null)
                foreach (var mediaItem in VideoItems)
                {
                    var userNames = mediaItem.GetUserNamesInMediaItem();
                    result.AddRange(userNames);
                }
            return result;
        }

        public override void SetUserIdOfUserTags(Dictionary<string, string> userTaggeds)
        {
            if (PhotoItems != null)
                foreach (var mediaItem in PhotoItems)
                {
                    mediaItem.SetUserIdOfUserTags(userTaggeds);
                }
            if (VideoItems != null)
                foreach (var mediaItem in VideoItems)
                {
                    mediaItem.SetUserIdOfUserTags(userTaggeds);
                }
        }
    }
}
