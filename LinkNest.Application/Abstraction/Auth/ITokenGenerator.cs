using LinkNest.Domain.Identity;

namespace LinkNest.Application.Abstraction.Auth
{
    public interface ITokenGenerator
    {
        Task<string> GenerateJwtTokenAsync(AppUser user);
        RefreshToken GenereteRefreshToken();
    }

}
