using Hiwu.SpecificationPattern.Application.Interfaces.Services;
using Hiwu.SpecificationPattern.Domain.Settings;
using Hiwu.SpecificationPattern.Shared.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Hiwu.SpecificationPattern.Shared
{
    public static class ServiceRegister
    {
        public static void AddSharedInfrastructure(this IServiceCollection services, IConfiguration _config)
        {
            services.Configure<MailSettings>(_config.GetSection("MailSettings"));
            services.AddTransient<IEmailService, EmailService>();
        }
    }
}
