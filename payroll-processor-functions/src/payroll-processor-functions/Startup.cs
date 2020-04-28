using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using PayrollProcessor.Functions;
using PayrollProcessor.Functions.Features.Resources;
using PayrollProcessor.Functions.Infrastructure;

[assembly: FunctionsStartup(typeof(Startup))]

namespace PayrollProcessor.Functions
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            JsonConvert.DefaultSettings = () => DefaultJsonSerializerSettings.JsonSerializerSettings;

            builder.Services.AddHttpClient<ApiClient>();
        }
    }
}
