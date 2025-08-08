using LinkNest.Application.Abstraction.Messaging;

namespace LinkNest.Application.UserProfiles.UpdateUserProfile
{
        public record UpdateUserProfileCommand(
            Guid Id,
            string FirstName,
            string LastName,
            string Email,
            DateTime DateOfBirth,
            string CurrentCity,
            string PhoneNumber
         ) : ICommand;
}
