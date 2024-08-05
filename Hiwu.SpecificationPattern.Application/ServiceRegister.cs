using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Hiwu.SpecificationPattern.Application
{
    public static class ServiceRegister
    {
        public static void AddApplicationLayer(this IServiceCollection services)
        {
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
        }
    }
}
