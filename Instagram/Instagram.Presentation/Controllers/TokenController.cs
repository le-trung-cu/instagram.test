using Microsoft.AspNetCore.Mvc;
using Service.Contracts;
using Shared.DataTransferObjects;

namespace Instagram.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        private readonly IServiceManager _serviceManager;

        public TokenController(IServiceManager serviceManager)
        {
            _serviceManager = serviceManager;
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> RefreshToken([FromBody] TokenDto tokenDto)
        {
            if(tokenDto.AccessToken == null)
            {
                var accessToken = Request.Cookies["ACCESS_TOKEN"];
                tokenDto = tokenDto with { AccessToken = accessToken };
            }
            if(tokenDto.RefreshToken == null)
            {
                var refreshToken = Request.Cookies["REFRESH_TOKEN"];
                tokenDto = tokenDto with { RefreshToken = refreshToken};
            }
            var tokenDtoToReturn = await _serviceManager.AuthenticationService.RefreshToken(tokenDto);
            return Ok(tokenDtoToReturn);
        }
    }
}