using LinkNest.Application.Abstraction.Messaging;
using LinkNest.Domain.Abstraction;
using LinkNest.Domain.Posts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkNest.Application.Posts.AddInteractionToPost
{
    internal class AddInteractionCommandHandler : ICommandHandler<AddInteractionCommand, Guid>
    {
        private readonly IUnitOfWork unitOfWork;

        public AddInteractionCommandHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public async Task<Result<Guid>> Handle(AddInteractionCommand request, CancellationToken cancellationToken)
        {
            var post = await unitOfWork.PostRep.GetByIdAsync(request.postId);
            if (post == null)
                return Result<Guid>.Failure(["No Post Found"]);

            var user = await unitOfWork.userProfileRepo.GetByIdAsync(request.userProfileId);
            if (user is null)
                return Result<Guid>.Failure(["No User Found"]);

            var interaction = PostInteraction.Create(request.postId, request.userProfileId, request.interactionType);
            post.AddInteraction(interaction);

            await unitOfWork.SaveChangesAsync();
            return Result<Guid>.Success(interaction.Guid);
        }
    }
}
