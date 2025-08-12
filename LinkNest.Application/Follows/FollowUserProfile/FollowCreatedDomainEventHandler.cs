using LinkNest.Application.Abstraction.IServices;
using LinkNest.Domain.Abstraction;
using LinkNest.Domain.Follows;
using LinkNest.Domain.Follows.DomainEvents;
using MediatR;

namespace LinkNest.Application.Follows.FollowUserProfile
{
    internal sealed class FollowCreatedDomainEventHandler : INotificationHandler<FollowCreatedDomainEvent>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IOneSignalService oneSignal;

        public FollowCreatedDomainEventHandler(IUnitOfWork unitOfWork,IOneSignalService _oneSignal)
        {
            this.unitOfWork = unitOfWork;
            oneSignal = _oneSignal;
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

            // هنا ExternalUserId هو نفس UserId
            var ExternalId = followeeUser.AppUserId.ToString();

            await oneSignal.SendNotificationAsync(
                ExternalId,
                "New follower",
                $"{followingUser.FullName} started following you",
                new { followerId = followingUser.Guid }
            );

        }
    }
}
