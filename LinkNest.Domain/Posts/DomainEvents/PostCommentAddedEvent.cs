using LinkNest.Domain.Abstraction;

namespace LinkNest.Domain.Posts.DomainEvents
{
    public record PostCommentAddedDomainEvent(Guid postId, Guid userProfileId):IDomainEvent;
}
