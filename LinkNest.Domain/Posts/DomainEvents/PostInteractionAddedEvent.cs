using LinkNest.Domain.Abstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkNest.Domain.Posts.DomainEvents
{
    public record PostInteractionAddedDomainEvent(Guid interactionId, Guid postId, Guid userProfileId, DateTime createdAt):IDomainEvent;
}
