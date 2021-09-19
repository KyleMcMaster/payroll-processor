using LanguageExt;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
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

var builder = WebApplication.CreateBuilder(args);

JsonConvert.DefaultSettings = () => DefaultJsonSerializerSettings.JsonSerializerSettings;

builder.Services.AddCors(options =>
    options.AddPolicy("CorsPolicy",
        cpb => cpb
            .WithOrigins(builder.Configuration["CORS:client:domain"])
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials()));

builder.Services
    .AddControllers(options => options.UseGlobalRoutePrefix("api/v{version:apiVersion}"))
    .AddNewtonsoftJson();

builder.Services.AddApiVersioning(options =>
{
    options.ReportApiVersions = true;
    options.DefaultApiVersion = new ApiVersion(1, 0);
});

builder.Services.AddSignalR();

builder.Services
    .AddCosmosClient(builder.Configuration)
    .AddQueueClient(builder.Configuration)
    .AddCQRSTypes();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "PayrollProcessor", Version = "v1" });
    c.EnableAnnotations();
});

builder.Services.AddMvc();

builder.Services.AddHealthChecks();

builder.Services.AddSingleton<IEntityIdGenerator, EntityIdGenerator>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

var logger = app.Logger;

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

await app.RunAsync();
