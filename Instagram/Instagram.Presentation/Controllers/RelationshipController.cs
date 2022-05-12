using Contracts;
using Entities.Exceptions;
using Entities.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Service.Contracts;
using Shared.RequestFeatures;

namespace Instagram.Presentation.Controllers;
[ApiController]
[Route("api/[controller]")]
[Authorize]
public class RelationshipController : ControllerBase
{
    private readonly IServiceManager _services;
    private readonly IRepositoryManager _repositoryManager;
    private readonly UserManager<User> _userManager;

    public RelationshipController(IServiceManager services, IRepositoryManager repository, UserManager<User> userManager)
    {
        _services = services;
        _repositoryManager = repository;
        _userManager = userManager;
    }

    [HttpPut("{userName}")]
    public async Task<IActionResult> FollowUser(string userName)
    {
        await _services.FollowService.CreateFollowAsync(User, userName);
        return NoContent();
    }

    [HttpDelete("{userName}")]
    public async Task<IActionResult> UnFollowUser(string userName)
    {
        await _services.FollowService.DeleteFollowAsync(User, userName);
        return NoContent();
    }

    [HttpGet("{followingId}")]
    public async Task<IActionResult> FollowStatus(string followingId)
    {
        var followerId = _userManager.GetUserId(User);
        var follow = await _repositoryManager.Follow.GetFollowAsync(followerId, followingId);
        if (follow == null)
        {
            return Ok(new { Status = "unfollow", Follow = (object?)null });
        }
        else
        {
            return Ok(new { Status = "following", Follow = follow });
        }
    }

    [HttpGet("suggestion")]
    public async Task<IActionResult> Suggestion([FromQuery] PageParameters parameters)
    {
        var (friends, metaData) = await _services.FollowService.GetSugestionFollowForUser(User, parameters);
        Response.Headers.AddXPagination(metaData);
        return Ok(friends);
    }

    [HttpGet("/api/{userName}/followers")]
    public async Task<IActionResult> GetFollowers(string userName, [FromQuery] PageParameters parameters)
    {
        var (items, meta) = await _services.FollowService.GetFollowers(User, userName, parameters);
        Response.Headers.AddXPagination(meta);
        return Ok(items);
    }

    [HttpGet("/api/{userName}/following")]
    public async Task<IActionResult> GetFollowing(string userName, [FromQuery] PageParameters parameters)
    {
        var (items, meta) = await _services.FollowService.GetFollowing(User, userName, parameters);
        Response.Headers.AddXPagination(meta);
        return Ok(items);
    }
}

