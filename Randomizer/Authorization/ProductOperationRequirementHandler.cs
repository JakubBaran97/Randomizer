using Microsoft.AspNetCore.Authorization;
using Randomizer.Models;
using System.Security.Claims;

namespace Randomizer.Authorization
{
    

        public class ProductOperationRequirementHandler : AuthorizationHandler<ProductOperationRequirement, Product>
        {
            protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ProductOperationRequirement requirement, Product product)
            {
               

                var userId = context.User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier).Value;

                if (product.CreatedById == int.Parse(userId))
                {
                    context.Succeed(requirement);
                }

                return Task.CompletedTask;
            }


        }
    }

