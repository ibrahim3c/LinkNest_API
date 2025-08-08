using LinkNest.Application.Abstraction.Helpers;
using LinkNest.Application.Abstraction.IServices;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace LinkNest.Infrastructure.Email
{
    internal sealed class EmailService : IEmailService
    {
        private readonly IOptionsMonitor<SendGridSettings> sendGridSettings;

        public EmailService(IOptionsMonitor<SendGridSettings> sendGridSettings)
        {
            this.sendGridSettings = sendGridSettings;
        }
        public async Task SendAsync(string mailTo, string subject, string body, IList<IFormFile> files = null)
        {
            var client = new SendGridClient(sendGridSettings.CurrentValue.ApiKey);
            var from = new EmailAddress(sendGridSettings.CurrentValue.SenderMail, sendGridSettings.CurrentValue.SenderName);
            var to = new EmailAddress(mailTo);

            var msg = MailHelper.CreateSingleEmail(from, to, subject, body, body);

            // Attach files if any
            if (files != null && files.Count > 0)
            {
                foreach (var attachment in files)
                {
                    using (var ms = new MemoryStream())
                    {
                        await attachment.CopyToAsync(ms);
                        var fileBytes = ms.ToArray();
                        msg.AddAttachment(attachment.FileName, Convert.ToBase64String(fileBytes));
                    }
                }
            }

            var response = await client.SendEmailAsync(msg);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Failed to send email: {response.StatusCode}");
            }
        }
    }
}
