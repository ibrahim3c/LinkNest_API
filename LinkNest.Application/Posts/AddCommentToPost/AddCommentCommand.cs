using LinkNest.Application.Abstraction.Messaging;
using LinkNest.Domain.Posts;

namespace LinkNest.Application.Posts.AddCommentToPost
{
    public record AddCommentCommand(string Content, Guid PostId, Guid UserProfileId):ICommand<Guid>;
}
