using LinkNest.Application.Abstraction.Messaging;
using LinkNest.Domain.Abstraction;
using LinkNest.Domain.Posts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkNest.Application.Posts.UpdateCommentToPost
{
    internal class UpdateCommentCommandHandler : ICommandHandler<UpdateCommentCommand>
    {
        private readonly IUnitOfWork unitOfWork;

        public UpdateCommentCommandHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public async Task<Result> Handle(UpdateCommentCommand request, CancellationToken cancellationToken)
        {
            var comment = await unitOfWork.PostRep.GetCommentByIdAsync(request.commandId);
            if (comment == null)
                return Result.Failure(["No Command Found"]);

            comment.UpdateConent(new Content( request.content));

            await unitOfWork.SaveChangesAsync();
            return  Result.Success();

        }
    }
}
