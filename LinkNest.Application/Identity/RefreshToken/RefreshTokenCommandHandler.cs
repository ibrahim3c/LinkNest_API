using LinkNest.Application.Abstraction.Auth;
using LinkNest.Application.Abstraction.Messaging;
using LinkNest.Domain.Abstraction;
using LinkNest.Domain.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace LinkNest.Application.Identity.RefreshToken
{
    internal record RefreshTokenCommandHandler : IAuthCommandHandler<RefreshTokenCommand>
    {
        private readonly UserManager<AppUser> userManager;
        private readonly ITokenGenerator tokenGenerator;

        public RefreshTokenCommandHandler(UserManager<AppUser> userManager,ITokenGenerator tokenGenerator)
        {
            this.userManager = userManager;
            this.tokenGenerator = tokenGenerator;
        }
        public async Task<AuthResult> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
        {
            // ensure there is user has this refresh token
            var user = await userManager.Users.SingleOrDefaultAsync(u => u.RefreshTokens.Any(r => r.Token == request.refreshToken));
            if (user == null)
            {
                return new AuthResult
                {
                    // u can don't add false=> cuz it's the default value 
                    Success = false,
                    Messages = ["InValid Token"]
                };
            }
            // ensure this token is active
            var oldRefreshToken = user.RefreshTokens.SingleOrDefault(t => t.Token == request.refreshToken);
            if (!oldRefreshToken.IsActive)
                return new AuthResult
                {
                    Success = false,
                    Messages = ["InValid Token"]
                };
            // if all things well
            //revoke old refresh token
            oldRefreshToken.RevokedOn = DateTime.UtcNow;

            // generate new refresh token and add it to db
            var newRefreshToken =tokenGenerator.GenereteRefreshToken();
            user.RefreshTokens.Add(newRefreshToken);
            await userManager.UpdateAsync(user);

            // generate new JWT Token
            var jwtToken = await tokenGenerator.GenerateJwtTokenAsync(user);

            return new AuthResult
            {
                Success = true,
                Messages = ["Refresh Token Successfully"],
                RefreshToken = newRefreshToken.Token,
                RefreshTokenExpiresOn = newRefreshToken.ExpiresOn,
                Token =jwtToken
            };

        }
    }
}
