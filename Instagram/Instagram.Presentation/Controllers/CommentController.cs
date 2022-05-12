using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Contracts;
using Shared.DataTransferObjects;
using Shared.RequestFeatures;

namespace Instagram.Presentation.Controllers
{
    [ApiController]
    [Route("api/posts/{slug}/comments")]
    [Authorize]
    public class CommentController : ControllerBase
    {
        private readonly IServiceManager _serviceManager;
        private readonly IPostSlugService _slugService;

        public CommentController(IServiceManager serviceManager, IPostSlugService slugService)
        {
            _serviceManager = serviceManager;
            _slugService = slugService;
        }

        [HttpGet]
        public async Task<IActionResult> GetComments(string slug, [FromQuery] CommentParameters commentParameters)
        {
            var postId = _slugService.Decode(slug);

            var (comments, metadata) = await _serviceManager.CommentService.GetCommentsForPostAsync(User, postId, commentParameters);
            HttpContext.Response.Headers.AddXPagination(metadata);
            return Ok(comments);
        }

        [HttpGet("/api/comments/{id}")]
        public async Task<IActionResult> GetComment(int id)
        {
            var comments = await _serviceManager.CommentService.GetCommentAsync(id);
            return Ok(comments);
        }

        [HttpPost]
        public async Task<IActionResult> CreateComment(string slug, CommentForCreationDto commentForCreation)
        {
            if(commentForCreation == null)
            {
                return BadRequest("CommentForCreationDto object is null");
            }
            if (!ModelState.IsValid)
            {
                return UnprocessableEntity(ModelState);
            }
            var postId = _slugService.Decode(slug);
            commentForCreation.PostId = postId;
            var comment = await _serviceManager.CommentService.CreateCommentAsync(User, commentForCreation);
            return CreatedAtAction(nameof(GetComment), new { id = comment.Id }, comment);
        }

        [HttpDelete("/api/comments/{id}")]
        public async Task<IActionResult> DeleteComment(int id)
        {
            await _serviceManager.CommentService.DeleteCommentAsync(User, id);
            return NoContent();
        }

        [HttpPut("/api/comments/{id}/like")]
        public async Task<IActionResult> LikeComment(int id)
        {
            await _serviceManager.LikeService.LikeCommentAsync(User, id);
            return Ok();
        }

        [HttpDelete("/api/comments/{id}/like")]
        public async Task<IActionResult> UnlikeComment(int id)
        {
            await _serviceManager.LikeService.UnLikeCommentAsync(User, id);
            return NoContent();
        }
    }
}
