using Microsoft.AspNetCore.Identity;

namespace LinkNest.Domain.Identity
{
    public class AppRole : IdentityRole
    {
        public ICollection<AppPermission> Permissions { get; set; } = new List<AppPermission>();
    }
}
