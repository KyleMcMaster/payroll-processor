using System;
using System.Net.Http;
using LanguageExt;
using Newtonsoft.Json;

namespace PayrollProcessor.Functions.Api.Infrastructure;

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
            Message = JsonConvert.SerializeObject(data)
        };

        return async () =>
        {
            var response = await client.PostAsJsonAsync("/api/v1/notification", notification);

            response.EnsureSuccessStatusCode();

            return Unit.Default;
        };
    }
}

public class Notification
{
    public string Source { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
}
