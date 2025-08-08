using LinkNest.Application.Abstraction.IServices;
using LinkNest.Domain.Abstraction;
using LinkNest.Domain.UserProfiles.DomainEvents;
using MediatR;

namespace LinkNest.Application.Identity.Register
{
    internal sealed class UserProfileCreatedDomainEventHandler : INotificationHandler<UserProfileCreatedDomainEvent>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IEmailService emailService;

        public UserProfileCreatedDomainEventHandler(IUnitOfWork unitOfWork,IEmailService emailService)
        {
            this.unitOfWork = unitOfWork;
            this.emailService = emailService;
        }
        public  async Task Handle(UserProfileCreatedDomainEvent notification, CancellationToken cancellationToken)
        {
            var user = await unitOfWork.userProfileRepo.GetByIdAsync(notification.userProfileId);
            if (user == null)
                return;
            await emailService.SendAsync(user.Email.ToString(), "Welcome!", "Thanks for registering.");

        }
    }
}
