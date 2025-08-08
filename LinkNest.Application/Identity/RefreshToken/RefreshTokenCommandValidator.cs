using FluentValidation;

namespace LinkNest.Application.Identity.RefreshToken
{
    internal class RefreshTokenCommandValidator:AbstractValidator<RefreshTokenCommand>
    {
        public RefreshTokenCommandValidator()
        {
            RuleFor(x => x.refreshToken).NotEmpty().WithMessage("RefreshToken is required");
        }
    }
}
