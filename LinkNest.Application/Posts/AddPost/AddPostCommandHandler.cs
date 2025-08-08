using LinkNest.Application.Abstraction.Messaging;
using LinkNest.Domain.Abstraction;
using LinkNest.Domain.Posts;

namespace LinkNest.Application.Posts.AddPost
{
    internal class AddPostCommandHandler : ICommandHandler<AddPostCommand, Guid>
    {
        private readonly IUnitOfWork unitOfWork;

        public AddPostCommandHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public async Task<Result<Guid>> Handle(AddPostCommand request, CancellationToken cancellationToken)
        {
            var user = await unitOfWork.userProfileRepo.GetByIdAsync(request.UserProfileId);
            if (user is null)
                return Result<Guid>.Failure(["No User Profile Found"]);

            var post = Post.Create(new Content( request.Content), new Url( request.ImageUrl), request.UserProfileId);
            await unitOfWork.PostRep.AddAsync(post);
            await unitOfWork.SaveChangesAsync();

            return Result<Guid>.Success(post.Guid);
        }
    }
}
