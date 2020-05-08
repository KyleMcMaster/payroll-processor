using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PayrollProcessor.Api.Features.Notifications;
using PayrollProcessor.Api.Infrastructure.Routing;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PayrollProcessor.Data.Persistence.Features.Employees;
using PayrollProcessor.Data.Domain.Intrastructure.Operations.Factories;
using System;
using PayrollProcessor.Data.Domain.Intrastructure.Operations.Queries;
using Microsoft.Azure.Cosmos;
using PayrollProcessor.Api.Configuration.Persistence;

namespace PayrollProcessor.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration) => Configuration = configuration;

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
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

            services.AddCosmosClient(Configuration);

            services.AddAutoMapper(typeof(DbContext).Assembly);

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "PayrollProcessor", Version = "v1" });
                c.EnableAnnotations();
            });

            services.AddTransient<ServiceProviderDelegate>(ctx => t => ctx.GetRequiredService(t));

            services.AddCQRSTypes();

            services.AddMvc();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseCors("CorsPolicy");

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();

                endpoints.MapHub<NotificationHub>("/hub/notifications");
            });

            app.UseCors();

            app.UseSwagger();

            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Payroll Processor API V1"));
        }
    }
}
