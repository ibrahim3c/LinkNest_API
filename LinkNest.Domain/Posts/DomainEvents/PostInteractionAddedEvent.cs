using LinkNest.Domain.Abstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkNest.Domain.Posts.DomainEvents
{
    //(interaction.Guid, interaction.PostId, interaction.UserProfileId, interaction.CreatedAt));
    public record PostInteractionAddedEvent(Guid interactionId, Guid postId, Guid userProfileId, DateTime createdAt):IDomainEvent;
}
