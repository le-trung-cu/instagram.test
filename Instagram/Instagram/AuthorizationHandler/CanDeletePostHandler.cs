using Entities.Models;
using Instagram.AuthorizationRequirement;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace Instagram.AuthorizationHandler
{
    public class CanDeletePostHandler : AuthorizationHandler<CanDeletePostRequirement, Post>
    {
        private readonly UserManager<User> _userManager;

        public CanDeletePostHandler(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, CanDeletePostRequirement requirement, Post resource)
        {
            var userId = _userManager.GetUserId(context.User);
            if (userId == resource.OwnerId || context.User.HasClaim(ClaimTypes.Role, "Admin"))
            {
                context.Succeed(requirement);
            }
            return Task.CompletedTask;
        }
    }
}
