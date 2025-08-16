using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;

namespace LinkNest.Infrastructure.Auth
{
    public class PermissionPolicyProvider : DefaultAuthorizationPolicyProvider
    {
        public PermissionPolicyProvider(IOptions<AuthorizationOptions> options)
            : base(options) { }

        public override Task<AuthorizationPolicy?> GetPolicyAsync(string policyName)
        {
            // if the policy name matches a Permission enum value
            if (Enum.TryParse<Permission>(policyName, out var permission))
            {
                var policy = new AuthorizationPolicyBuilder()
                    .AddRequirements(new PermissionRequirement(policyName))
                    .Build();

                return Task.FromResult<AuthorizationPolicy?>(policy);
            }

            // fallback to default provider (for "Admin", "User", etc.)
            return base.GetPolicyAsync(policyName);
        }
    }
}
