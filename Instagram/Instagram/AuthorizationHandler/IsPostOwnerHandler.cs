using Entities.Models;
using Instagram.AuthorizationRequirement;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace Instagram.AuthorizationHandler
{
    public class IsPostOwnerHandler : AuthorizationHandler<IsPostOwnerRequirement, Post>
    {
        private readonly UserManager<User> _userManager;

        public IsPostOwnerHandler(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, IsPostOwnerRequirement requirement, Post resource)
        {
            var userId = _userManager.GetUserId(context.User);
            if(userId == resource.OwnerId)
            {
                context.Succeed(requirement);
            }
            return Task.CompletedTask;
        }
    }
}
