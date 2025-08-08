using LinkNest.Application.Abstraction.Messaging;

namespace LinkNest.Application.Identity.Login
{
    public record LoginCommand(string Email , string Password ):IAuthCommand;
}
