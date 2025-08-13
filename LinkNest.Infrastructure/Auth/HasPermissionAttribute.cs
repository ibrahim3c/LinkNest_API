using Microsoft.AspNetCore.Authorization;

namespace LinkNest.Infrastructure.Auth
{
    public sealed class HasPermissionAttribute:AuthorizeAttribute
    {
        public HasPermissionAttribute(Permission permission) : base(permission.ToString())
        {
        }
        // This class can be extended with additional properties or methods if needed
        // For example, you might want to add logging or custom error handling here
    }
}
