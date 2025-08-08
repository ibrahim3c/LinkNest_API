using LinkNest.Application.Abstraction.Messaging;

namespace LinkNest.Application.Identity.Register
{
    public record RegisterCommand(
        string Fname,
        string Lname,
        string CurrentCity,
        string PhoneNumber,
        DateTime BirthDate,
        string Email,
        string Password,
        string ConfirmPassword
    ) : IAuthCommand;
}


