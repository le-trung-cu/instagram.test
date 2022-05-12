namespace Service.Contracts
{
    public interface IRelationShipService
    {
        Task<IEnumerable<string>> GetFriendIdsAsync(string userId);
    }
}
