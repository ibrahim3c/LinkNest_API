using LinkNest.Application.Abstraction.IServices;
using LinkNest.Domain.Abstraction;
using LinkNest.Domain.Posts.DomainEvents;
using MediatR;

namespace LinkNest.Application.Posts.AddInteractionToPost
{
    internal class PostInteractionAddedDomainEventHandler : INotificationHandler<PostInteractionAddedDomainEvent>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IOneSignalService oneSignalService;

        public PostInteractionAddedDomainEventHandler(IUnitOfWork unitOfWork,IOneSignalService oneSignalService)
        {
            this.unitOfWork = unitOfWork;
            this.oneSignalService = oneSignalService;
        }
        public async Task Handle(PostInteractionAddedDomainEvent notification, CancellationToken cancellationToken)
        {
            var post = await unitOfWork.PostRep.GetByIdAsync(notification.postId);
            if (post == null)
                return;

            var user = await unitOfWork.userProfileRepo.GetByIdAsync(notification.userProfileId);
            if (user is null)
                return;

            var interactedUser = await unitOfWork.userProfileRepo.GetByIdAsync(post.UserProfileId);
            if (interactedUser is null)
                return;

            // هنا ExternalUserId هو نفس UserId
            var ExternalId = user.Guid.ToString();

            await oneSignalService.SendNotificationAsync(
                ExternalId,
                "New Interaction on Your Post",
               $"{interactedUser.FirstName} interact on your post."
            );
        }
    }
}
