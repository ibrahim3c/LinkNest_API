using LinkNest.Domain.UserProfiles;
using Microsoft.AspNetCore.Identity;

namespace LinkNest.Domain.Identity
{
    public class AppUser : IdentityUser
    {
        public ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();

    }
}
