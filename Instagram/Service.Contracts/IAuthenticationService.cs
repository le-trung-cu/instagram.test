using Microsoft.AspNetCore.Identity;
using Shared.DataTransferObjects;

namespace Service.Contracts
{
    public interface IAuthenticationService
    {
        Task<IdentityResult> RegisterUserAsync(UserForRegistrationDto userForRegistration);
        Task<bool> VerifyUserAsync(UserForAuthenticationDto userForAuthentication);
        Task<TokenDto> CreateToken(bool populateExp);
        Task<TokenDto> RefreshToken(TokenDto token);
    }
}
