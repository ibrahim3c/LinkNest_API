using FluentValidation;

namespace LinkNest.Application.Posts.UpdateCommentToPost
{
    internal class UpdateCommentCommandValidator : AbstractValidator<UpdateCommentCommand>
    {
        public UpdateCommentCommandValidator()
        {
            RuleFor(x => x.commandId)
                .NotEmpty().WithMessage("Command ID is required.");

            RuleFor(x => x.content)
                .NotEmpty().WithMessage("Content is required.")
                .MaximumLength(300).WithMessage("Content cannot be more than 300 characters.");
        }
    }
}
