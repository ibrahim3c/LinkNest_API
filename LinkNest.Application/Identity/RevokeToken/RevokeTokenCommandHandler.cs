using LinkNest.Application.Abstraction.Messaging;
using LinkNest.Domain.Abstraction;
using LinkNest.Domain.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace LinkNest.Application.Identity.RevokeToken
{

    internal class RevokeTokenCommandHandler : ICommandHandler<RevokeTokenCommand>
    {
        private readonly UserManager<AppUser> userManager;

        public RevokeTokenCommandHandler(UserManager<AppUser> userManager)
        {
            this.userManager = userManager;
        }
        public async Task<Result> Handle(RevokeTokenCommand request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(request.refreshToken))
                return Result.Failure(["RefreshToken is required"]);

            var user = await userManager.Users.SingleOrDefaultAsync(u => u.RefreshTokens.Any(r => r.Token == request.refreshToken));
            if (user == null)
                return Result.Failure(["User not found"]);

            var oldRefreshToken = user.RefreshTokens.SingleOrDefault(t => t.Token == request.refreshToken);
            if (!oldRefreshToken.IsActive)
                return Result.Failure(["InValid Token"]);


            oldRefreshToken.RevokedOn = DateTime.UtcNow;


            await userManager.UpdateAsync(user);

            return Result.Success();
        }
    }
}
