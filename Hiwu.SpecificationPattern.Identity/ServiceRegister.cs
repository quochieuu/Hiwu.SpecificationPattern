using Hiwu.SpecificationPattern.Application.Interfaces.Services;
using Hiwu.SpecificationPattern.Application.Wrappers;
using Hiwu.SpecificationPattern.Domain.Settings;
using Hiwu.SpecificationPattern.Identity.Contexts;
using Hiwu.SpecificationPattern.Identity.Entities;
using Hiwu.SpecificationPattern.Identity.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.Text;
using static Hiwu.SpecificationPattern.Domain.Common.Constants;

namespace Hiwu.SpecificationPattern.Identity
{
    public static class ServiceRegister
    {
        public static void AddIdentityInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<IdentityContext>(options => options.UseNpgsql(
                            configuration.GetConnectionString("IdentityConnection")));

            services.AddIdentity<ApplicationUser, ApplicationRole>().AddEntityFrameworkStores<IdentityContext>().AddDefaultTokenProviders();
            services.AddTransient<IAccountService, AccountService>();

            services.Configure<JWTSettings>(configuration.GetSection("JWTSettings"));
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(o =>
                {
                    o.RequireHttpsMetadata = false;
                    o.SaveToken = false;
                    o.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.Zero,
                        ValidIssuer = configuration["JWTSettings:Issuer"],
                        ValidAudience = configuration["JWTSettings:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWTSettings:Key"]))
                    };
                    o.Events = new JwtBearerEvents()
                    {
                        OnAuthenticationFailed = context =>
                        {
                            // Check has force logout
                            context.NoResult();
                            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                            context.Response.ContentType = "application/json";
                            var error = new ResponseFailure
                            {
                                ErrorCode = ErrorCodes.ApiKeyInvalid,
                                ErrorMessage = "You are not Authorized",
                            };

                            if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
                            {
                                context.Response.Headers.Add("Token-Expired", "true");
                                error = new ResponseFailure
                                {
                                    ErrorCode = ErrorCodes.ApiKeyInvalid,
                                    ErrorMessage = "The access token provided has expired.",
                                };
                            }
                            var result = JsonConvert.SerializeObject(error);
                            context.Response.WriteAsync(result);
                            return Task.CompletedTask;
                        },
                        OnChallenge = context =>
                        {
                            context.HandleResponse();
                            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                            context.Response.ContentType = "application/json";
                            var error = new ResponseFailure
                            {
                                ErrorCode = ErrorCodes.ApiKeyInvalid,
                                ErrorMessage = "You are not Authorized",
                            };
                            var result = JsonConvert.SerializeObject(error);
                            context.Response.WriteAsync(result);
                            return Task.CompletedTask;
                        },
                        OnForbidden = context =>
                        {
                            context.Response.StatusCode = 403;
                            context.Response.ContentType = "application/json";
                            var error = new ResponseFailure
                            {
                                ErrorCode = "resourceForbidden",
                                ErrorMessage = "You are not authorized to access this resource",
                            };
                            var result = JsonConvert.SerializeObject(error);
                            return context.Response.WriteAsync(result);
                        },
                    };
                });
        }
    }
}
