using Hiwu.SpecificationPattern.Core;
using Hiwu.SpecificationPattern.Domain.Database;
using Hiwu.SpecificationPattern.Generic;
using Hiwu.SpecificationPattern.SignalR;
using Hiwu.SpecificationPattern.SignalR.Hubs;
using Microsoft.EntityFrameworkCore;
using System.Threading;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Register health checks
builder.Services.AddHealthChecks();

// Register the app context
builder.Services.AddDbContext<AppDbContext>(options => options.UseNpgsql(
                            builder.Configuration.GetConnectionString("ConnectionString")));

// Register repository
builder.Services.ApplyHiwuRepository<AppDbContext>();

// Register signalr
builder.Services.AddSignalRServices();

// Register core layer
builder.Services.AddApplicationLayer();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapHealthChecks("/health");

app.UseRouting();

app.UseEndpoints(endpoints =>
{
    endpoints.MapHub<ChatHub>("/chathub");
    endpoints.MapHub<NotificationHub>("/notificationhub");
});

app.UseAuthorization();

app.MapControllers();

app.Run();
