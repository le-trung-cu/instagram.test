using Microsoft.AspNetCore.Mvc;
using Service.Contracts;
using Shared.DataTransferObjects;

namespace Instagram.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IServiceManager _service;

        public AuthenticationController(IServiceManager service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<ActionResult> RegisterUser([FromBody] UserForRegistrationDto userForRegistration)
        {
            var result = await _service.AuthenticationService.RegisterUserAsync(userForRegistration);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.TryAddModelError(error.Code, error.Description);
                }
                return BadRequest(result);
            }
            return StatusCode(201);
        }

        [HttpPost("login")]
        public async Task<ActionResult> Authenticate([FromBody] UserForAuthenticationDto userForAuthentication)
        {
            if (!await _service.AuthenticationService.VerifyUserAsync(userForAuthentication))
            {
                return Unauthorized();
            }
            return Ok(await _service.AuthenticationService.CreateToken(populateExp: true));
        }
    }
}
