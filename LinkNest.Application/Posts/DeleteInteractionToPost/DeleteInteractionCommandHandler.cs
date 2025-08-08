using LinkNest.Application.Abstraction.Messaging;
using LinkNest.Domain.Abstraction;

namespace LinkNest.Application.Posts.DeleteInteractionToPost
{
    internal class DeleteInteractionCommandHandler : ICommandHandler<DeleteInteractionCommand>
    {
        private readonly IUnitOfWork unitOfWork;

        public DeleteInteractionCommandHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public async Task<Result> Handle(DeleteInteractionCommand request, CancellationToken cancellationToken)
        {
            var interaction = await unitOfWork.PostRep.GetInteractionByIdAsync(request.interactionId);

            if (interaction is null)
                return Result.Failure(["No Interaction Found"]);

            var post = await unitOfWork.PostRep.GetByIdAsync(interaction.PostId, p => p.Interactions);
            if (post is null)
                return Result.Failure(["No Post Found"]);

            post.RemoveInteraction(interaction);
            await unitOfWork.SaveChangesAsync();

            return Result.Success();
        }
    }
}
