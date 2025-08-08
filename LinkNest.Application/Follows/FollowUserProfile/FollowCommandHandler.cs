using LinkNest.Application.Abstraction.Messaging;
using LinkNest.Domain.Abstraction;
using LinkNest.Domain.Follows;

namespace LinkNest.Application.Follows.FollowUserProfile
{
    internal class FollowCommandHandler : ICommandHandler<FollowCommand>
    {
        private readonly IUnitOfWork unitOfWork;

        public FollowCommandHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public async Task<Result> Handle(FollowCommand request, CancellationToken cancellationToken)
        {
            if (request.followingId == request.followeeId)
                return Result.Failure(["You cannot follow yourself."]);

            var followingUser = await unitOfWork.userProfileRepo.GetByIdAsync(request.followingId);
            var followeeUser = await unitOfWork.userProfileRepo.GetByIdAsync(request.followeeId);

            if (followeeUser == null || followingUser == null)
                return Result.Failure(["Invalid user(s)."]);

            if (await unitOfWork.followRepo.IsFollowingAsync(request.followingId, request.followeeId))
                return Result.Failure(["Uou already following this User"]);
            
            var follow=Follow.Create(request.followingId,request.followeeId);
            await unitOfWork.followRepo.AddAsync(follow);
            await unitOfWork.SaveChangesAsync();

            return Result.Success();

        }
    }
}
