using Microsoft.AspNetCore.Http;

namespace LinkNest.Application.Abstraction.IServices
{
    public interface IEmailService
    {
        Task SendAsync(string mailTo, string subject, string body, IList<IFormFile> files = null);
    }
}
