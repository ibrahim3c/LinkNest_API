using LinkNest.Application.Abstraction.Messaging;

namespace LinkNest.Application.Identity.RevokeToken
{
    public record RevokeTokenCommand(string refreshToken):ICommand;
}
