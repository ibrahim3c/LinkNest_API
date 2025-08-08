using LinkNest.Domain.Abstraction;

namespace LinkNest.Domain.Posts.DomainEvents
{
    public record PostCommentAddedDomainEvent(Guid commentId, Guid postId, Guid userProfileId, Content Content ,DateTime createdAt):IDomainEvent;
}
