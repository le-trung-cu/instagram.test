using Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Contracts;
using Shared.RequestFeatures;
using Shared.DataTransferObjects;
using Marvin.Cache.Headers;

namespace Instagram.Presentation.Controllers;

[Route("api/posts")]
[ApiController]
[Authorize]
public class PostController : ControllerBase
{
    private readonly ILoggerManager _logger;
    private readonly IServiceManager _service;
    private readonly IPostSlugService _slugService;

    public PostController(ILoggerManager logger, IServiceManager service, IPostSlugService slugService)
    {
        _logger = logger;
        _service = service;
        _slugService = slugService;
    }

    [HttpGet("/api/{username}/posts")]
    //[HttpCacheExpiration(CacheLocation = CacheLocation.Private, MaxAge = 60)]
    //[HttpCacheValidation(MustRevalidate = false)]
    public async Task<IActionResult> GetPosts(string username, [FromQuery] PostParameters postParameters)
    {
        if (postParameters.Thumbnail)
        {
            var (posts, metaData) = await _service.PostService.GetPostThumbnailsOfUserAsync(username, postParameters);
            var result = posts.Select(t => (object)t).ToList();
            Response.Headers.AddXPagination(metaData);

            return Ok(result);
        }
        else
        {
            var (posts, metaData) = await _service.PostService.GetPostsAsync(username, postParameters, User);
            var result = posts.Select(t => (object)t).ToList();
            Response.Headers.AddXPagination(metaData);

            return Ok(result);
        }
    }

    [HttpGet("explore")]
    public async Task<IActionResult> ExplorePosts([FromQuery] PostParameters parameters)
    {
        var(posts, meta) = await _service.PostService.GetExplorePostsAsync(parameters);
        Response.Headers.AddXPagination(meta);
        return Ok(posts);
    }

    [HttpGet("{slug}")]
    //[HttpCacheExpiration(CacheLocation = CacheLocation.Private, MaxAge = 60)]
    //[HttpCacheValidation(MustRevalidate = false)]
    public async Task<IActionResult> GetPost(string slug)
    {
        var id = _slugService.Decode(slug);
        var post = await _service.PostService.GetPostAsync(id, User);
        return Ok(post);
    }

    [HttpGet("/api/posts/saved")]
    public async Task<IActionResult> GetSavedPosts([FromQuery] PostParameters postParameters)
    {
        var (posts, metaData) = await _service.PostService.GetSavedPostsOfUser(User, postParameters);
        Response.Headers.AddXPagination(metaData);
        return Ok(posts);
    }

    [HttpPost]
    public async Task<IActionResult> CreatePost([FromForm] PostBaseForCreationDto postForCreation)
    {
        if (postForCreation == null)
        {
            return BadRequest("PostForCreationDto object is null");
        }
        if (!ModelState.IsValid)
        {
            return UnprocessableEntity(ModelState);
        }
        var post = await _service.PostService.CreatePostAsync(postForCreation, User);
        return CreatedAtAction(nameof(GetPost), new { slug = post.Slug }, post);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdatePost(int id, [FromForm] PostForUpdateDto postForUpdate)
    {
        if (postForUpdate == null)
        {
            return BadRequest("PostForUpdateDto object is null");
        }
        if (!ModelState.IsValid)
        {
            return UnprocessableEntity(ModelState);
        }
        await _service.PostService.UpdatePostAsync(id, postForUpdate);
        return NoContent();
    }

    [HttpDelete("{slug}")]
    public async Task<IActionResult> DeletePost(string slug)
    {
        var id = _slugService.Decode(slug);
        await _service.PostService.DeletePostAsync(id, User);
        return NoContent();
    }

    [HttpPut("{slug}/like")]
    public async Task<IActionResult> LikePost(string slug)
    {
        var id = _slugService.Decode(slug);
        await _service.LikeService.LikePostAsync(User, id);
        return NoContent();
    }

    [HttpDelete("{slug}/like")]
    public async Task<IActionResult> UnlikePost(string slug)
    {
        var id = _slugService.Decode(slug);
        await _service.LikeService.UnLikePost(User, id);
        return NoContent();
    }

    [HttpPut("{slug}/save")]
    public async Task<IActionResult> SavePost(string slug)
    {
        var id = _slugService.Decode(slug);
        await _service.PostService.SavePost(User, id);
        return NoContent();
    }

    [HttpDelete("{slug}/save")]
    public async Task<IActionResult> DeleteSavedPost(string slug)
    {
        var id = _slugService.Decode(slug);
        await _service.PostService.DeleteSavedPost(User, id);
        return NoContent();
    }

    [HttpGet("/api/{userName}/tagged")]
    public async Task<IActionResult> GetMediasOfUser(string userName, [FromQuery] PostParameters parameters)
    {
        var (medias, metadata) = await _service.PostService.GetMediasOfUserAsync(userName, parameters);
        Response.Headers.AddXPagination(metadata);
        return Ok(medias);
    }
}
