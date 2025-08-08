using FluentValidation;
using LinkNest.Application.Posts.UpdateCommentToPost;

namespace LinkNest.Application.Posts.UpdatePostContent
{
    internal class UpdatePostContentCommandValidator : AbstractValidator<UpdatePostContentCommand>
    {
        public UpdatePostContentCommandValidator()
        {
            RuleFor(x => x.postId)
                .NotEmpty().WithMessage("Post ID is required.");

            RuleFor(x => x.Content)
                .NotEmpty().WithMessage("Content is required.")
                .MaximumLength(300).WithMessage("Content cannot be more than 300 characters.");
        }
    }
}
