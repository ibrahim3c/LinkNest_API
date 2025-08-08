using LinkNest.Application.Abstraction.Messaging;

namespace LinkNest.Application.Posts.UpdateCommentToPost
{
    public record UpdateCommentCommand(Guid commandId,string content):ICommand;
}
