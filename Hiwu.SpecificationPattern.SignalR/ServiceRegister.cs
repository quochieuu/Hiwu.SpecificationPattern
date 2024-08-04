using Microsoft.Extensions.DependencyInjection;

namespace Hiwu.SpecificationPattern.SignalR
{
    public static class ServiceRegister
    {
        public static IServiceCollection AddSignalRServices(this IServiceCollection services)
        {
            services.AddSignalR();

            return services;
        }
    }
}
