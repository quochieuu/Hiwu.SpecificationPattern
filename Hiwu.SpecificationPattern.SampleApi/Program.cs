using Hiwu.SpecificationPattern.Application.Interfaces.Repositories;
using Hiwu.SpecificationPattern.Core.Middlewares;
using Hiwu.SpecificationPattern.Generic;
using Hiwu.SpecificationPattern.Persistence.Database;
using Hiwu.SpecificationPattern.Application;
using Hiwu.SpecificationPattern.SignalR;
using Hiwu.SpecificationPattern.SignalR.Hubs;
using Hiwu.SpecificationPattern.Caching;
using Hiwu.SpecificationPattern.Shared;
using Hiwu.SpecificationPattern.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using AspNetCoreRateLimit;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(swg =>
{
    swg.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "Hiwu.SpecificationPattern.SampleAPI",
        Description = "This is sample API use PostgreSQL.",
        Contact = new OpenApiContact
        {
            Name = "Hiwu",
            Email = "quochieuu@gmail.com",
            Url = new Uri("https://quochieu.com/"),
        }
    });
    swg.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        Description = "Input your Bearer token in this format - Bearer {your token here} to access this API",
    });
    swg.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer",
                },
                Scheme = "Bearer",
                Name = "Bearer",
                In = ParameterLocation.Header,
            }, new List<string>()
        },
    });
});

// Register health checks
builder.Services.AddHealthChecks();

// Register the app context
builder.Services.AddDbContext<AppDbContext>(options => options.UseNpgsql(
                            builder.Configuration.GetConnectionString("ConnectionString")));

// Register repository
builder.Services.AddApplicationServices(builder.Configuration);

// Register caching
builder.Services.AddCacheService(builder.Configuration);

// Register signalr
builder.Services.AddSignalRServices();

// Register core layer
builder.Services.AddApplicationLayer();

// Register shared layer
builder.Services.AddSharedInfrastructure(builder.Configuration);

// Register identity layer
builder.Services.AddIdentityInfrastructure(builder.Configuration);

// Register and configure API versioning in the ASP.NET Core service container
builder.Services.AddApiVersioning(config =>
{
    // Set the default API version to 1.0
    config.DefaultApiVersion = new ApiVersion(1, 0);

    // Assume the default version (1.0) if no version is specified in the request
    config.AssumeDefaultVersionWhenUnspecified = true;

    // Report the API versions supported by the API in the response headers
    config.ReportApiVersions = true;
});

// Register rate limiting
builder.Services.AddMemoryCache();
builder.Services.Configure<IpRateLimitOptions>(builder.Configuration.GetSection("IpRateLimiting"));
builder.Services.Configure<IpRateLimitPolicies>(builder.Configuration.GetSection("IpRateLimitPolicies"));
builder.Services.AddSingleton<IProcessingStrategy, AsyncKeyLockProcessingStrategy>();
builder.Services.AddSingleton<IRateLimitCounterStore, MemoryCacheRateLimitCounterStore>();
builder.Services.AddSingleton<IIpPolicyStore, MemoryCacheIpPolicyStore>();
builder.Services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapHealthChecks("/health");

app.UseIpRateLimiting();

app.UseMiddleware<AppMiddleware>();

app.UseRouting();

app.UseEndpoints(endpoints =>
{
    endpoints.MapHub<ChatHub>("/chathub");
    endpoints.MapHub<NotificationHub>("/notificationhub");
});

app.UseAuthorization();

app.MapControllers();

app.Run();
