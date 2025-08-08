using LinkNest.Application.Abstraction.Messaging;

namespace LinkNest.Application.Identity.RefreshToken
{
    public record RefreshTokenCommand(string refreshToken):IAuthCommand;
}
