using LinkNest.Application.UserProfiles.GetUserProfile;

namespace LinkNest.Application.Follows.GetAllFollowers
{
    public class GetAllFollowersResponse
    {
        public Guid UserProfileId { get; init; }
        public List<GetUserProfileResponse> FollowersInfo { get; init; }
    }
}
