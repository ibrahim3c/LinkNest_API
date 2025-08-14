using Microsoft.AspNetCore.Authorization;

namespace LinkNest.Infrastructure.Auth
{
    public class PermissionRequirement : IAuthorizationRequirement
    {
        public PermissionRequirement() { }
        public PermissionRequirement(string permissionName)
        {
            PermissionName = permissionName;
        }
        public string PermissionName { get; } = string.Empty;

    }
}
