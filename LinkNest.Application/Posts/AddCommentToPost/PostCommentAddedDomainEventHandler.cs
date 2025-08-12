using LinkNest.Application.Abstraction.IServices;
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
        private readonly IOneSignalService oneSignalService;

        public PostCommentAddedDomainEventHandler(IUnitOfWork unitOfWork,IOneSignalService oneSignalService)
        {
            this.unitOfWork = unitOfWork;
            this.oneSignalService = oneSignalService;
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
            // هنا ExternalUserId هو نفس UserId
            var ExternalId = postOwnerId.ToString();

            await oneSignalService.SendNotificationAsync(
                ExternalId,
                "New Comment on Your Post",
               $"{commenter.FirstName} commented on your post."
            );
        }
    }
}
