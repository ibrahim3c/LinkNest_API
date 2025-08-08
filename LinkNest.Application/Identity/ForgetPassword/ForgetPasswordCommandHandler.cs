using LinkNest.Application.Abstraction.IServices;
using LinkNest.Application.Abstraction.Messaging;
using LinkNest.Domain.Abstraction;
using LinkNest.Domain.Identity;
using Microsoft.AspNetCore.Identity;

namespace LinkNest.Application.Identity.ForgetPassword
{
    internal class ForgetPasswordCommandHandler : ICommandHandler<ForgetPasswordCommand, string>
    {
        private readonly UserManager<AppUser> userManager;
        private readonly IEmailService emailService;

        public ForgetPasswordCommandHandler(UserManager<AppUser> userManager,IEmailService emailService)
        {
            this.userManager = userManager;
            this.emailService = emailService;
        }
        public async Task<Result<string>> Handle(ForgetPasswordCommand request, CancellationToken cancellationToken)
        {
            var user = await userManager.FindByEmailAsync(request.Email);
            if (user == null)
                return Result<string>.Failure(["Email is incorrect!"]);
            // generete token and  send it to user
            //await SendPasswordResetEmailAsync(user, scheme, host);
            await SendResetPasswordEmailAsync(user);


            return Result<string>.Success("Please go to your email and reset your password");

            //    // after that user click on link and go to frontend page that
            //    //1-capture userId, code
            //    //2-make form for user to reset new password
            //    // then user send data to reset password endpoint


        }

        private async Task SendResetPasswordEmailAsync(AppUser user)
        {
            // Generate the password reset token
            var code = await userManager.GeneratePasswordResetTokenAsync(user);

            // Construct the reset link
            var callbackUrl = $"https://full-stack-website-react-asp-net-eight.vercel.app/reset-password?userId={user.Id}&code={Uri.EscapeDataString(code)}";
            // Send email with the reset link
            await emailService.SendAsync(user.Email, "Reset Your Password",
                $"Please reset your password by clicking this link: <a href='{callbackUrl}'>Reset Password</a>");
        }
    }
}
