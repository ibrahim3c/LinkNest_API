using LinkNest.Domain.Abstraction;
using LinkNest.Domain.Posts.DomainEvents;
using MediatR;

namespace LinkNest.Application.Posts.AddInteractionToPost
{
    internal class PostInteractionAddedDomainEventHandler : INotificationHandler<PostInteractionAddedDomainEvent>
    {
        private readonly IUnitOfWork unitOfWork;

        public PostInteractionAddedDomainEventHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public async Task Handle(PostInteractionAddedDomainEvent notification, CancellationToken cancellationToken)
        {
            var post = await unitOfWork.PostRep.GetByIdAsync(notification.postId);
            if (post == null)
                return;

            var user = await unitOfWork.userProfileRepo.GetByIdAsync(notification.userProfileId);
            if (user is null)
                return;
        }
    }
}
