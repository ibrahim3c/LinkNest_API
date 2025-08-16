using LinkNest.Application.Abstraction.Helpers;
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
        // first way using permission service
        //protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
        //{
        //    // get userId from context
        //    var userId = context.User.Claims.FirstOrDefault(c => c.Type == "uid")?.Value;
        //    if (userId == null)
        //    {
        //        return ;
        //    }

        //    HashSet<string> permissions = await permissionService.GetPermissionsAsync(userId);
        //    if (permissions.Contains(requirement.PermissionName))
        //    {
        //        context.Succeed(requirement);
        //        return ;
        //    }
        //}

        // second way using claims directly
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
        {
            var permissions = context.User.Claims
                .Where(c => c.Type == Constants.PermissionsKey)?
                .Select(p => p.Value)
                .ToHashSet();

            if (permissions.Contains(requirement.PermissionName))
            {
                context.Succeed(requirement);
            }
            return Task.CompletedTask;
        }
    }
}
