using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Payroll.Processor.Functions.Features.Resources;
using Payroll.Processor.Functions;
using Payroll.Processor.Functions.Infrastructure;

[assembly: FunctionsStartup(typeof(Startup))]

namespace Payroll.Processor.Functions
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
