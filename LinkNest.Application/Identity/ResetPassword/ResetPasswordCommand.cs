using LinkNest.Application.Abstraction.Messaging;

namespace LinkNest.Application.Identity.ResetPassword
{
    public record ResetPasswordCommand(string userId,string code,string newPassword):ICommand<string>;
}
