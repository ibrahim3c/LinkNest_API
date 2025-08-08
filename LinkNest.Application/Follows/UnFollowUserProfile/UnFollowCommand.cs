using LinkNest.Application.Abstraction.Messaging;

namespace LinkNest.Application.Follows.UnFollowUserProfile
{
    public record UnFollowCommand(Guid followeeId, Guid followerId) : ICommand;

}
