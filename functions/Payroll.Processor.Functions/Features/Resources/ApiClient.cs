using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Payroll.Processor.Functions.Infrastructure;

namespace Payroll.Processor.Functions.Features.Resources
{
    public class ApiClient
    {
        private readonly HttpClient client;

        public ApiClient(HttpClient client)
        {
            string apiDomain = EnvironmentSettings.Get("API_Domain")
                .IfNone(() => "http://localhost:5000");

            client.BaseAddress = new Uri(apiDomain);

            this.client = client;
        }

        public async Task SendNotification<T>(string source, T data)
        {
            var notification = new Notification
            {
                Source = source,
                Message = JsonConvert.SerializeObject(data, DefaultJsonSerializerSettings.JsonSerializerSettings)
            };

            var response = await client.PostAsJsonAsync("/api/notification", notification);

            return;
        }
    }

    public class Notification
    {
        public string Source { get; set; } = "";
        public string Message { get; set; } = "";
    }
}
