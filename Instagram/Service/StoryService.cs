using Microsoft.Extensions.Caching.Distributed;
using Service.Contracts;
using Shared.DataTransferObjects;
using System.Text.Json;

namespace Service
{
    public class StoryService : IStoryService
    {
        private readonly IDistributedCache _distributedCache;

        public StoryService(IDistributedCache distributedCache)
        {
            _distributedCache = distributedCache;
        }

        public async Task<IEnumerable<StoryDto>> GetStoriesAsync(string userId)
        {
            var storyIds = await GetStoryIdsAsync(userId);
            var stories = new List<StoryDto>();
            foreach (var storyId in storyIds)
            {
                var cachedStory = await _distributedCache.GetStringAsync("story:" + storyId);
                var story = JsonSerializer.Deserialize<StoryDto>(cachedStory)!;
                stories.Add(story);
            }
            return stories;
        }

        private async Task<List<int>> GetStoryIdsAsync(string userId)
        {
            var cachedStoryIds = await _distributedCache.GetStringAsync("storyIds:" + userId);
            var storyIds = JsonSerializer.Deserialize<List<int>>(cachedStoryIds) ?? new List<int>();
            return storyIds;
        }

        public async Task SetStoryAsync(StoryDto story)
        {
            await _distributedCache.SetStringAsync("story:" + story.PostId, JsonSerializer.Serialize(story));
        }

        private async Task SetSrotydsAsync(string userId, IEnumerable<int> storyIds)
        {
            await _distributedCache.SetStringAsync("storyIds:" + userId, JsonSerializer.Serialize(storyIds));
        }

        public async Task PushStoryIdAsync(string userId, int storyId)
        {
            throw new NotImplementedException();
            var storyIds = await GetStoryIdsAsync(userId);
            storyIds.Add(storyId);
            await SetSrotydsAsync(userId, storyIds);
        }
    }
}
