using LinkNest.Application.Abstraction.Messaging;

namespace LinkNest.Application.Follows.GetAllFollowers
{
    public sealed record GetAllFollowersQuery(Guid UserProfileId):IQuery<GetAllFollowersResponse>;
}
