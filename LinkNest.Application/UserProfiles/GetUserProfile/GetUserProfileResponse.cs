using ApartmentBooking.Domain.Users;
using LinkNest.Domain.UserProfiles;

namespace LinkNest.Application.UserProfiles.GetUserProfile
{
    public class GetUserProfileResponse
    {
        public Guid UserProfileId { get; init; }
        public string FirstName { get; init; }
        public string LastName { get; init; }
        public string Email { get; init; }
        public DateTime DateOfBirth { get;init; }
        public DateTime CreatedOn { get; init; }
        public string CurrentCity { get; init; }
        public string PhoneNumber { get; init; }

    }
}
