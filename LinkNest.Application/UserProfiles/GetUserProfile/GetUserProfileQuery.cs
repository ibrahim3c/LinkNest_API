using LinkNest.Application.Abstraction.Messaging;

namespace LinkNest.Application.UserProfiles.GetUserProfile
{
    public sealed record GetUserProfileQuery(Guid userProfileId):IQuery<GetUserProfileResponse>;
}
