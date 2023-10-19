using Microsoft.AspNetCore.Authorization;

namespace Randomizer.Authorization
{
    public enum MenuResourceOperation
    {
        Create,
        Update,
        Delete,
        Read
    }
    public class MenuOperationRequirement : IAuthorizationRequirement
    {
        public MenuOperationRequirement(MenuResourceOperation resourceOperation)
        {
            ResourceOperation = resourceOperation;
        }
        public MenuResourceOperation ResourceOperation { get; }
    }
}
