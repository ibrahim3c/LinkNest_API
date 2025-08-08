using LinkNest.Domain.Abstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkNest.Domain.Posts.DomainEvents
{
    public record PostCommentAddedEvent(Guid commentId, Guid postId, Guid userProfileId, Content Content ,DateTime createdAt):IDomainEvent;
}
