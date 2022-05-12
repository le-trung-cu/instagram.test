using Contracts;
using Entities.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Service.Contracts;
using Shared.RequestFeatures;

namespace Instagram.Presentation.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class FeedController : ControllerBase
{
    private readonly ILoggerManager _logger;
    private readonly IServiceManager _service;
    private readonly UserManager<User> _usermanager;

    public FeedController(ILoggerManager logger,
        IServiceManager service,
        UserManager<User> usermanager)
    {
        _logger = logger;
        _service = service;
        _usermanager = usermanager;
    }

    [HttpGet("posts")]
    public async Task<IActionResult> GetPots([FromQuery] PostParameters parameters)
    {
        var (posts, meta) = await _service.PostService.GetFeedPostsAsync(User, parameters);
        Response.Headers.AddXPagination(meta);
        return Ok(posts);
    }
}
