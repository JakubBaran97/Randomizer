using Microsoft.AspNetCore.Authorization;

namespace Randomizer.Authorization
{
    public enum ProductResourceOperation
    {
        Create,
        Update,
        Delete,
        Read
    }
    public class ProductOperationRequirement : IAuthorizationRequirement
    {
        public ProductOperationRequirement(ProductResourceOperation resourceOperation)
        {
            ResourceOperation = resourceOperation;
        }
        public ProductResourceOperation ResourceOperation { get; }
    }
}
