using LinkNest.Application.Abstraction.Messaging;

namespace LinkNest.Application.Follows.GetAllFollowees
{
    public sealed record GetAllFolloweesQuery(Guid userProfileId):IQuery<GetAllFolloweesRespones>;
}
