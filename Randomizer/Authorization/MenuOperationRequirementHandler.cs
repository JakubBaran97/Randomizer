using Microsoft.AspNetCore.Authorization;
using Randomizer.Models;
using System.Security.Claims;
using static Randomizer.Authorization.MenuOperationRequirement;

namespace Randomizer.Authorization
{
    public class MenuOperationRequirementHandler : AuthorizationHandler<MenuOperationRequirement, Menu>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, MenuOperationRequirement requirement, Menu menu)
        {
            if (requirement.ResourceOperation == MenuResourceOperation.Read ||
              requirement.ResourceOperation == MenuResourceOperation.Create)
            {
                context.Succeed(requirement);
            }

            var userId = context.User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier).Value;

            if (menu.CreatedById == int.Parse(userId))
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }

        
    }
}

