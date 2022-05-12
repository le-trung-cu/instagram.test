namespace Contracts
{
    public interface IActivityUserDataCountRepository
    {
        Task DescreaseFollowerCount(string userId);
        Task DescreaseFollowingCount(string userId);
        Task DescreasePostCount(string userId);
        Task IncreaseFollowerCount(string userId);
        Task IncreaseFollowingCount(string userId);
        Task IncreasePostCount(string userId);
    }
}
