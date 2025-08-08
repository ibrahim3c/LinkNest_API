using LinkNest.Application.Abstraction.Messaging;
using LinkNest.Domain.Abstraction;
using LinkNest.Domain.Posts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkNest.Application.Posts.UpdatePostContent
{
    internal class UpdatePostContentCommandHandler : ICommandHandler<UpdatePostContentCommand>
    {
        private readonly IUnitOfWork unitOfWork;

        public UpdatePostContentCommandHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public async Task<Result> Handle(UpdatePostContentCommand request, CancellationToken cancellationToken)
        {
            var post = await unitOfWork.PostRep.GetByIdAsync(request.postId);
            if (post == null)
                return Result.Failure(["No Post Found"]);

            post.UpdateContent(new Content( request.Content));
            await unitOfWork.SaveChangesAsync();

            return Result.Success();

        }
    }
}
