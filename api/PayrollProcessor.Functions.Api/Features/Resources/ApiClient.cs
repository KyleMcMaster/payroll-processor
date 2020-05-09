using System;
using System.Net.Http;
using LanguageExt;
using Newtonsoft.Json;
using PayrollProcessor.Functions.Api.Infrastructure;

namespace PayrollProcessor.Functions.Api.Features.Resources
{
    public interface IApiClient
    {
        TryOptionAsync<Unit> SendNotification<T>(string source, T data);
    }

    public class ApiClient : IApiClient
    {
        private readonly HttpClient client;

        public ApiClient(HttpClient client)
        {
            string apiDomain = EnvironmentSettings.Get("API_Domain")
                .IfNone(() => "http://localhost:5000");

            client.BaseAddress = new Uri(apiDomain);

            this.client = client;
        }

        public TryOptionAsync<Unit> SendNotification<T>(string source, T data)
        {
            var notification = new Notification
            {
                Source = source,
                Message = JsonConvert.SerializeObject(data, DefaultJsonSerializerSettings.JsonSerializerSettings)
            };

            return async () =>
            {
                var response = await client.PostAsJsonAsync("/api/notification", notification);

                response.EnsureSuccessStatusCode();

                return Unit.Default;
            };
        }
    }

    public class Notification
    {
        public string Source { get; set; } = "";
        public string Message { get; set; } = "";
    }
}
