using LinkNest.Application.Abstraction.Messaging;
using LinkNest.Domain.Abstraction;
using LinkNest.Domain.Identity;
using Microsoft.AspNetCore.Identity;

namespace LinkNest.Application.Identity.ResetPassword
{
    internal class ResetPasswordCommandHandler : ICommandHandler<ResetPasswordCommand, string>
    {
        private readonly UserManager<AppUser> userManager;

        public ResetPasswordCommandHandler(UserManager<AppUser> userManager)
        {
            this.userManager = userManager;
        }
        public async Task<Result<string>> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(request.userId) || string.IsNullOrWhiteSpace(request.code))
            {
                return Result<string>.Failure(["UserId and code are required"]);
            }

            var user = await userManager.FindByIdAsync(request.userId);
            if (user == null)
            {
                return Result<string>.Failure(["User not found"]);
            }

            // Decode the token before using it
            var decodedCode = Uri.UnescapeDataString(request.code);

            var result = await userManager.ResetPasswordAsync(user, decodedCode, request.newPassword);
            if (result.Succeeded)
            {
                return Result<string>.Success("Password Reset successfully");
            }
            else
            {
                return Result<string>.Failure(["Error resetting password."]);
            }

        }
    }
}
