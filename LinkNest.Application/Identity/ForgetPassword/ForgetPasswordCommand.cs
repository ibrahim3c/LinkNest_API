using LinkNest.Application.Abstraction.Messaging;

namespace LinkNest.Application.Identity.ForgetPassword
{
    public record ForgetPasswordCommand(string Email):ICommand<string>; 
}
