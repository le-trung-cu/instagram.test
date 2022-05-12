using Shared.DataTransferObjects;

namespace Service.Contracts
{
    public interface IStoryService
    {
        Task<IEnumerable<StoryDto>> GetStoriesAsync(string userId);
        Task PushStoryIdAsync(string userId, int storyId);
        Task SetStoryAsync(StoryDto story);
    }
}