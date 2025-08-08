using LinkNest.Domain.Abstraction;
using LinkNest.Domain.Posts.DomainEvents;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkNest.Application.Posts.AddCommentToPost
{
    internal sealed class PostCommentAddedDomainEventHandler : INotificationHandler<PostCommentAddedDomainEvent>
    {
        private readonly IUnitOfWork unitOfWork;

        public PostCommentAddedDomainEventHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public async Task Handle(PostCommentAddedDomainEvent notification, CancellationToken cancellationToken)
        {
            // Step 1: Get the post to find the owner
            var post = await unitOfWork.PostRep.GetByIdAsync(notification.postId);
            if (post == null)
                return;

            var postOwnerId = post.UserProfileId;

            // Step 2: Skip notification if commenter is the post owner
            if (postOwnerId == notification.userProfileId)
                return;

            // Step 3: Get commenter's profile
            var commenter = await unitOfWork.userProfileRepo.GetByIdAsync(notification.userProfileId);
            if (commenter == null)
                return;

            // Step 4 : Send Notification 
        }
    }
}
