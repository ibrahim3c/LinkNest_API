using LinkNest.Application.Abstraction.Messaging;
using LinkNest.Domain.Posts;

namespace LinkNest.Application.Posts.UpdatePostContent
{
    public record UpdatePostContentCommand(Guid postId,string Content):ICommand;
}
