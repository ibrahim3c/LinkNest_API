using LinkNest.Application.Abstraction.Messaging;

namespace LinkNest.Application.Posts.DeleteCommentToPost
{
    public record DeleteCommentCommand(Guid CommentId):ICommand;
}
