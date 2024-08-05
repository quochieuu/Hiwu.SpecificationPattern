using Hiwu.SpecificationPattern.Application.Interfaces.Repositories;
using Hiwu.SpecificationPattern.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Hiwu.SpecificationPattern.Generic
{
    public static class ServiceRegister
    {
        public static IServiceCollection ApplyHiwuRepository<TDbContext>(this IServiceCollection services, ServiceLifetime serviceLifetime = ServiceLifetime.Transient) 
            where TDbContext : DbContext
        {
            services.Add(new ServiceDescriptor(
                typeof(IRepository),
                serviceProvider =>
                {
                    var dbContext = ActivatorUtilities.CreateInstance<TDbContext>(serviceProvider);
                    return new Repository(dbContext);
                },
                serviceLifetime));

            services.Add(new ServiceDescriptor(
                typeof(IUnitOfWork),
                serviceProvider =>
                {
                    var repository = serviceProvider.GetService<IRepository>();
                    var productRepository = serviceProvider.GetService<IProductRepository>();
                    return new UnitOfWork(repository, productRepository);
                },
                serviceLifetime));
            return services;
        }
    }
}
