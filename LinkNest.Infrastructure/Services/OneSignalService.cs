using LinkNest.Application.Abstraction.Helpers;
using LinkNest.Application.Abstraction.IServices;
using Microsoft.Extensions.Options;
using System.Text;
using System.Text.Json;

namespace LinkNest.Infrastructure.Services
{
    public class OneSignalService : IOneSignalService
    {
        private readonly HttpClient _httpClient;
        private readonly OneSignalOptions _options;

        public OneSignalService(HttpClient httpClient, IOptions<OneSignalOptions> options)
        {
            _httpClient = httpClient;
            _options = options.Value;
            _httpClient.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", _options.ApiKey);
        }

        public async Task SendNotificationAsync(string externalUserId, string title, string message, object data = null)
        {
            if (string.IsNullOrWhiteSpace(externalUserId))
                throw new ArgumentException("User ID cannot be empty", nameof(externalUserId));

            if (string.IsNullOrWhiteSpace(title))
                throw new ArgumentException("Title cannot be empty", nameof(title));

            var payload = new
            {
                app_id = _options.AppId,
                include_external_user_ids = new[] { externalUserId },
                headings = new { en = title },
                contents = new { en = message },
                data
            };

            var json = JsonSerializer.Serialize(payload);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("https://onesignal.com/api/v1/notifications", content);
            var result = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"OneSignal error: {result}");
            }
        }
    }
}
