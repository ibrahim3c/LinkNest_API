namespace LinkNest.Application.Abstraction.IServices
{
    public interface IOneSignalService
    {
        Task SendNotificationAsync(string externalUserId, string title, string message, object data = null);
    }
}
