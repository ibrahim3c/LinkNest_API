using LinkNest.Application.Abstraction.Messaging;

namespace LinkNest.Application.Follows.FollowUserProfile
{
    public record FollowCommand(Guid followeeId, Guid followingId):ICommand;
}
