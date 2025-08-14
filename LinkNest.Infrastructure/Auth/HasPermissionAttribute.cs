using Microsoft.AspNetCore.Authorization;

namespace LinkNest.Infrastructure.Auth
{
    public sealed class HasPermissionAttribute:AuthorizeAttribute
    {
        public HasPermissionAttribute(Permission permission) : base(permission.ToString())
        {
        }
     }
}
