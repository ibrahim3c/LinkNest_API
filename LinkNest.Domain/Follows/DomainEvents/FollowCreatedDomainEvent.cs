using LinkNest.Domain.Abstraction;

namespace LinkNest.Domain.Follows.DomainEvents
{
    public record class FollowCreatedDomainEvent(Guid FolloweeId, Guid FollowerId):IDomainEvent;
}
