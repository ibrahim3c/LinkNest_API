namespace LinkNest.Api.Controllers.V1.UserProfiles
{
    public record UpdateUserProfileRequest(
        string FirstName,
        string LastName,
        string Email,
        DateTime DateOfBirth,
        string CurrentCity,
        string PhoneNmber);
}
