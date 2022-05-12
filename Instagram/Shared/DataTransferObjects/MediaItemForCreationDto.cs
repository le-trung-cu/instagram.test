using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Shared.DataTransferObjects;

public abstract record MediaItemForCreationDto
{
    [BindNever]
    public string? Path { get; set; }
    public IFormFile? File { get; init; }
    public string? ModelType { get; init; }
    public abstract IEnumerable<string> GetUserNamesInMediaItem();
    public abstract void SetUserIdOfUserTags(Dictionary<string, string> userTaggeds);

}
public record PhotoItemForCreationDto : MediaItemForCreationDto
{
    public IEnumerable<UserTagPhotoForCreationDto>? UserTags { get; init; }
    public override IEnumerable<string> GetUserNamesInMediaItem()
    {
        return UserTags?.Select(t => t.UserName) ?? Enumerable.Empty<string>();
    }

    public override void SetUserIdOfUserTags(Dictionary<string, string> userTaggeds)
    {
        if (UserTags != null)
        {
            foreach (var userTag in UserTags)
            {
                userTag.UserId = userTaggeds[userTag.UserName];
            }
        }
    }
}
public record VideoItemForCreationDto : MediaItemForCreationDto
{
    public IEnumerable<UserTagVideoForCreationDto>? UserTags { get; init; }
    public override IEnumerable<string> GetUserNamesInMediaItem()
    {
        return UserTags?.Select(t => t.UserName) ?? Enumerable.Empty<string>();
    }

    public override void SetUserIdOfUserTags(Dictionary<string, string> userTaggeds)
    {
        if (UserTags != null)
        {
            foreach (var userTag in UserTags)
            {
                userTag.UserId = userTaggeds[userTag.UserName];
            }
        }
    }
}