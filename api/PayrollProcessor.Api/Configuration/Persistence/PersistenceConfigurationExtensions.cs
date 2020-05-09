using System;
using Microsoft.Azure.Cosmos;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PayrollProcessor.Core.Domain.Intrastructure.Operations.Factories;
using PayrollProcessor.Core.Domain.Intrastructure.Operations.Queries;
using PayrollProcessor.Data.Persistence.Features.Employees;
using PayrollProcessor.Data.Persistence.Infrastructure.Clients;

namespace PayrollProcessor.Api.Configuration.Persistence
{
    public static class PersistenceConfigurationExtensions
    {
        public static void AddCosmosEFCore(this IServiceCollection services, IConfiguration configuration) =>
            services.AddDbContext<EmployeesContext>(options =>
            {
                string serviceEndpoint = configuration.GetValue<string>("CosmosDb:ServiceEndpoint");
                string authKey = configuration.GetValue<string>("CosmosDb:AuthKey");
                string databaseName = configuration.GetValue<string>("CosmosDb:DatabaseName");

                options.UseCosmos(serviceEndpoint, authKey, databaseName);
            });

        public static IServiceCollection AddCosmosClient(this IServiceCollection services, IConfiguration configuration) =>
            services.AddSingleton(ctx =>
            {
                string serviceEndpoint = configuration.GetValue<string>("CosmosDb:ServiceEndpoint");
                string authKey = configuration.GetValue<string>("CosmosDb:AuthKey");

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

        public static IServiceCollection AddQueueClient(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IQueueClientFactory>(ctx =>
            {
                string connectionString = configuration.GetValue<string>("AzureStorageQueue:ConnectionString");

                return new QueueClientFactory(connectionString);
            });

            return services;
        }

        public static IServiceCollection AddCQRSTypes(this IServiceCollection services)
        {
            services.AddTransient<ServiceProviderDelegate>(ctx => t => ctx.GetRequiredService(t));

            services.Scan(scan => scan
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

            return services;
        }
    }
}
