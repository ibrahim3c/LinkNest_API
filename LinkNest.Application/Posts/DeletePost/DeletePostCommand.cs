
using LinkNest.Application.Abstraction.Messaging;

namespace LinkNest.Application.Posts.DeletePost
{
    public record DeletePostCommand(Guid postId):ICommand;
}
