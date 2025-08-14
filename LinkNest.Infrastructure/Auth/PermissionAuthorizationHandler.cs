using LinkNest.Application.Abstraction.IServices;
using Microsoft.AspNetCore.Authorization;

namespace LinkNest.Infrastructure.Auth
{
    public class PermissionAuthorizationHandler : AuthorizationHandler<PermissionRequirement>
    {
        private readonly IPermissionService permissionService;

        public PermissionAuthorizationHandler(IPermissionService permissionService)
        {
            this.permissionService = permissionService;
        }
        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
        {
            // first way using permission service
            // get userId from context
            var userId = context.User.Claims.FirstOrDefault(c => c.Type == "uid")?.Value;
            if (userId == null)
            {
                return ;
            }

            HashSet<string> permissions = await permissionService.GetPermissionsAsync(userId);
            if (permissions.Contains(requirement.PermissionName))
            {
                context.Succeed(requirement);
                return ;
            }
        }
    }
}
