using LinkNest.Application.Abstraction.Messaging;

namespace LinkNest.Application.Posts.DeleteInteractionToPost
{
    public record DeleteInteractionCommand(Guid interactionId):ICommand;
}
