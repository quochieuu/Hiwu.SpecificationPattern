using Hiwu.SpecificationPattern.Domain.Database;
using Hiwu.SpecificationPattern.Generic;
using Microsoft.EntityFrameworkCore;

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

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapHealthChecks("/health");

app.UseAuthorization();

app.MapControllers();

app.Run();
