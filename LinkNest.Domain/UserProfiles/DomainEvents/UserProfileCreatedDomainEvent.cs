using LinkNest.Domain.Abstraction;

namespace LinkNest.Domain.UserProfiles.DomainEvents
{
    public record UserProfileCreatedDomainEvent(Guid userProfileId):IDomainEvent;
}
