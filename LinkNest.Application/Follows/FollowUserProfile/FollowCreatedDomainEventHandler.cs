using LinkNest.Domain.Abstraction;
using LinkNest.Domain.Follows.DomainEvents;
using MediatR;

namespace LinkNest.Application.Follows.FollowUserProfile
{
    internal sealed class FollowCreatedDomainEventHandler : INotificationHandler<FollowCreatedDomainEvent>
    {
        private readonly IUnitOfWork unitOfWork;

        public FollowCreatedDomainEventHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public  async Task Handle(FollowCreatedDomainEvent notification, CancellationToken cancellationToken)
        {
            if (notification.FollowerId == notification.FolloweeId)
                return;

            var followingUser = await unitOfWork.userProfileRepo.GetByIdAsync(notification.FollowerId);
            var followeeUser = await unitOfWork.userProfileRepo.GetByIdAsync(notification.FolloweeId);

            if (followeeUser == null || followingUser == null)
                return ;

            if (await unitOfWork.followRepo.IsFollowingAsync(notification.FollowerId, notification.FolloweeId))
                return ;

            // send notification to followee that follower with name follows u : to do : learn ASP.NET Core push notifications OneSignal

        }
    }
}
