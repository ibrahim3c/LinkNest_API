using FluentValidation;

namespace LinkNest.Application.Posts.AddPost
{
    internal class AddPostCommandValidator : AbstractValidator<AddPostCommand>
    {
        public AddPostCommandValidator()
        {
            RuleFor(x => x.Content)
                .NotEmpty().WithMessage("Content is required.")
                .MaximumLength(500).WithMessage("Content cannot be longer than 500 characters.");

            RuleFor(x => x.ImageUrl)
                .NotEmpty().WithMessage("Image URL is required.")
                .Must(url => Uri.IsWellFormedUriString(url, UriKind.Absolute))
                .WithMessage("Image URL must be a valid URL.");

            RuleFor(x => x.UserProfileId)
                .NotEmpty().WithMessage("User profile ID is required.");
        }
    }
}
