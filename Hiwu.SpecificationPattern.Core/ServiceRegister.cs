using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Hiwu.SpecificationPattern.Core
{
    public static class ServiceRegister
    {
        public static void AddApplicationLayer(this IServiceCollection services)
        {
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
        }
    }
}
