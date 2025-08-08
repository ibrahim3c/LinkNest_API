namespace LinkNest.Api.Controllers.V1.Accounts
{
    public record ResetPasswordRequest(string userId, string code, string newPassword);
}
