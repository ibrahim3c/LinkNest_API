using LinkNest.Domain.Abstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkNest.Domain.Posts.DomainEvents
{
    public record PostCreatedEvent(Guid postId,Content Content,DateTime createdAt, Url imageUrl, Guid userProfileId  ):IDomainEvent;
}
