using LinkNest.Application.Abstraction.Messaging;
using LinkNest.Domain.Posts;

namespace LinkNest.Application.Posts.AddInteractionToPost
{
    public record AddInteractionCommand(Guid postId, Guid userProfileId, InteractionTypes interactionType):ICommand<Guid>;
}
