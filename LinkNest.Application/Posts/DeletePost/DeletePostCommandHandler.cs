using LinkNest.Application.Abstraction.Messaging;
using LinkNest.Domain.Abstraction;

namespace LinkNest.Application.Posts.DeletePost
{
    internal class DeletePostCommandHandler : ICommandHandler<DeletePostCommand>
    {
        private readonly IUnitOfWork unitOfWork;

        public DeletePostCommandHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public async Task<Result> Handle(DeletePostCommand request, CancellationToken cancellationToken)
        {
            var post=await unitOfWork.PostRep.GetByIdAsync(request.postId);
            if (post == null)
                return Result.Failure(["No Post Found"]);

            unitOfWork.PostRep.Delete(post);
            await unitOfWork.SaveChangesAsync();

            return Result.Success();
        }
    }
}
