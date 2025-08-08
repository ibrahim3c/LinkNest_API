using LinkNest.Domain.Abstraction;
using LinkNest.Domain.Posts.DomainEvents;
using MediatR;

namespace LinkNest.Application.Posts.AddPost
{
    internal class PostAddedDomainEventHandler : INotificationHandler<PostCommentAddedDomainEvent>
    {
        private readonly IUnitOfWork unitOfWork;

        public PostAddedDomainEventHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public async Task Handle(PostCommentAddedDomainEvent notification, CancellationToken cancellationToken)
        {
            var user = await unitOfWork.userProfileRepo.GetByIdAsync(notification.userProfileId);
            if (user is null)
                return ;
        }
    }
}
