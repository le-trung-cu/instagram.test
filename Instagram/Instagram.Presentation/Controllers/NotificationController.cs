using Contracts;
using Entities.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Shared.RequestFeatures;

namespace Instagram.Presentation.Controllers;

[ApiController]
[Route("api/notification")]
[Authorize]
public class NotificationController : ControllerBase
{
    private readonly IRepositoryManager _repository;
    private readonly UserManager<User> _userManager;

    public NotificationController(IRepositoryManager repository, UserManager<User> userManager)
    {
        _repository = repository;
        _userManager = userManager;
    }

    [HttpGet]
    public async Task<IActionResult> GetNotification([FromQuery] PageParameters parameters)
    {
        var userId = _userManager.GetUserId(User);
        var notification = await _repository.NotificationRepository.GetNotifiCations(userId, parameters);

        Response.Headers.AddXPagination(notification.MetaData);
        return Ok(notification);
    }
}

