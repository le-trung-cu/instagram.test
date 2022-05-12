using Contracts;
using Entities.Models;
using Instagram.AuthorizationRequirement;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace Instagram.AuthorizationHandler
{
    public class CanDeleteCommentHandler : AuthorizationHandler<CanDeleteCommentRequirement, Comment>
    {
        private readonly IRepositoryManager _repository;
        private readonly UserManager<User> _userManager;

        public CanDeleteCommentHandler(IRepositoryManager repository, UserManager<User> userManager)
        {
            _repository = repository;
            _userManager = userManager;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context,
            CanDeleteCommentRequirement requirement, Comment resource)
        {
            var userId = _userManager.GetUserId(context.User);

            if (userId != null && (userId == resource.OwnerId || context.User.HasClaim(ClaimTypes.Role, "Admin")))
            {
                context.Succeed(requirement);
            }
            else
            {
                var post = await _repository.Post.GetPostAsync(resource.PostId);
                if (post != null && post.OwnerId == userId)
                {
                    context.Succeed(requirement);
                }
            }
        }
    }
}
