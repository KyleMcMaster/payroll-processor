using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using PayrollProcessor.Functions.Api;
using PayrollProcessor.Functions.Api.Infrastructure;
using Microsoft.Azure.Cosmos;
using PayrollProcessor.Core.Domain.Infrastructure.Serialization;
using PayrollProcessor.Data.Persistence.Infrastructure.Clients;
using PayrollProcessor.Core.Domain.Intrastructure.Operations.Factories;
using PayrollProcessor.Data.Persistence.Features.Employees;
using System;
using PayrollProcessor.Core.Domain.Intrastructure.Operations.Queries;

[assembly: FunctionsStartup(typeof(Startup))]

namespace PayrollProcessor.Functions.Api
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            JsonConvert.DefaultSettings = () => DefaultJsonSerializerSettings.JsonSerializerSettings;

            builder.Services.AddHttpClient<ApiClient>();
            builder.Services.AddSingleton(serviceProvider =>
            {
                string serviceEndpoint = EnvironmentSettings.Get(AppResources.CosmosDb.ServiceEndpoint)
                    .IfNone(() => "https://localhost:8081");

                string authKey = EnvironmentSettings.Get(AppResources.CosmosDb.AuthKey)
                    .IfNone(() => "C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==");

                return new CosmosClient(serviceEndpoint, authKey, new CosmosClientOptions
                {
                    SerializerOptions = new CosmosSerializationOptions
                    {
                        IgnoreNullValues = false,
                        Indented = false,
                        PropertyNamingPolicy = CosmosPropertyNamingPolicy.CamelCase
                    }
                });
            });

            builder.Services.AddTransient<ServiceProviderDelegate>(ctx => t => ctx.GetRequiredService(t));

            builder.Services.Scan(scan => scan
                .FromAssemblies(typeof(EmployeeRecord).Assembly, typeof(QueryDispatcher).Assembly)
                .AddClasses(classes => classes
                    .Where(t =>
                    {
                        if (!t.IsClass || t.IsAbstract)
                        {
                            return false;
                        }

                        string name = t.Name;

                        return name.EndsWith("QueryHandler", StringComparison.Ordinal) ||
                            name.EndsWith("CommandHandler", StringComparison.Ordinal) ||
                            name.EndsWith("Dispatcher", StringComparison.Ordinal);
                    }))
                    .AsImplementedInterfaces()
                    .WithScopedLifetime());

        }
    }
}
