using LanguageExt;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using PayrollProcessor.Core.Domain.Infrastructure.Serialization;
using PayrollProcessor.Core.Domain.Intrastructure.Identifiers;
using PayrollProcessor.Web.Api.Configuration.Persistence;
using PayrollProcessor.Web.Api.Features.Notifications;
using PayrollProcessor.Web.Api.Infrastructure.Routing;

namespace PayrollProcessor.Web.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration) => Configuration = configuration;

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            JsonConvert.DefaultSettings = () => DefaultJsonSerializerSettings.JsonSerializerSettings;

            services.AddCors(options =>
                options.AddPolicy("CorsPolicy",
                    builder => builder
                        .WithOrigins(Configuration.GetValue<string>("CORS:client:domain"))
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials()));

            services.AddControllers(options => options.UseGlobalRoutePrefix("api/v{version:apiVersion}"));

            services.AddApiVersioning(options =>
            {
                options.ReportApiVersions = true;
                options.DefaultApiVersion = new ApiVersion(1, 0);
            });

            services.AddSignalR();

            services
                .AddCosmosClient(Configuration)
                .AddQueueClient(Configuration)
                .AddCQRSTypes();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "PayrollProcessor", Version = "v1" });
                c.EnableAnnotations();
            });

            services.AddMvc();

            services.AddHealthChecks();

            services.AddSingleton<IEntityIdGenerator, EntityIdGenerator>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            var logger = loggerFactory.CreateLogger("TryOptionAsync Fault");

            TryConfig.ErrorLogger = ex => logger.LogError(ex, "Failure");

            app.UseRouting();

            app.UseCors("CorsPolicy");

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();

                endpoints.MapHealthChecks("");

                endpoints.MapHub<NotificationHub>("/hub/notifications");
            });

            app.UseCors();

            app.UseSwagger();

            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Payroll Processor API V1"));
        }
    }
}
