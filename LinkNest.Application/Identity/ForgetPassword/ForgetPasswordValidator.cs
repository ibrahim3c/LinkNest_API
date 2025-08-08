using FluentValidation;

namespace LinkNest.Application.Identity.ForgetPassword
{
    internal class ForgetPasswordValidator:AbstractValidator<ForgetPasswordCommand>
    {
        public ForgetPasswordValidator()
        {
            RuleFor(x => x.Email).NotEmpty().WithMessage("Email is required");
        }
    }
}
