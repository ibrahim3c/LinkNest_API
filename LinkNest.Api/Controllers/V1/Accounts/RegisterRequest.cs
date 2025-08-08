namespace LinkNest.Api.Controllers.V1.Accounts
{
    public record RegisterRequest
    (
        string Fname,
        string Lname,
        string CurrentCity,
        string PhoneNumber,
        DateTime BirthDate,
        string Email,
        string Password,
        string ConfirmPassword
    );
}
