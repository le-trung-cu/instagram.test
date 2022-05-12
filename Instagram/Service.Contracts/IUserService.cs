using Shared.DataTransferObjects;

namespace Service.Contracts
{
    public interface IUserService
    {
        Task<UserDto> GetUserByUserNameAsync(string userName);
    }
}
