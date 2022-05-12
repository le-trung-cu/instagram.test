using Contracts;
using Microsoft.AspNetCore.Mvc;
using Service.Contracts;

namespace Instagram.Presentation.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UserController : ControllerBase
    {
        private readonly ISearchRepository _searchRepository;
        private readonly IServiceManager _serviceManager;

        public UserController(ISearchRepository searchRepository, IServiceManager serviceManager)
        {
            _searchRepository = searchRepository;
            _serviceManager = serviceManager;
        }

        [HttpGet("search")]
        public async Task<IActionResult> SearchUser([FromQuery] string userName)
        {
            var users = await _searchRepository.SearchUserByUserName(userName);
            return Ok(users);
        }

        [HttpGet("{userName}")]
        public async Task<IActionResult> GetUser(string userName)
        {
            var userDto = await _serviceManager.UserService.GetUserByUserNameAsync(userName);
            return Ok(userDto);
        }
    }
}
