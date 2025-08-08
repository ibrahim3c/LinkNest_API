using LinkNest.Application.Abstraction.Messaging;
using LinkNest.Domain.Abstraction;

namespace LinkNest.Application.Follows.UnFollowUserProfile
{
    internal class UnFollowCommandHandler : ICommandHandler<UnFollowCommand>
    {
        private readonly IUnitOfWork unitOfWork;

        public UnFollowCommandHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public async Task<Result> Handle(UnFollowCommand request, CancellationToken cancellationToken)
        {
            var follow = await unitOfWork.followRepo.GetFollowAsync(request.followerId, request.followeeId);
            if (follow == null)
                return Result.Failure(["You are not following this User Profile"]);

            unitOfWork.followRepo.Remove(follow);
            await unitOfWork.SaveChangesAsync();

            return Result.Success();
        }
    }
}
