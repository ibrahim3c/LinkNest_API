using LinkNest.Domain.Posts;

namespace LinkNest.Api.Controllers.V1.Posts
{
    public record AddInteractionRequest(Guid postId, Guid userProfileId, InteractionTypes interactionType);
}
