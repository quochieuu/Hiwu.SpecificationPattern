using Hiwu.SpecificationPattern.Application.Interfaces.Caching;
using Hiwu.SpecificationPattern.Caching.MemoryCache;
using Hiwu.SpecificationPattern.Caching.Redis;
using Hiwu.SpecificationPattern.Domain.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Hiwu.SpecificationPattern.Caching
{
    public static class ServiceRegister
    {
        public static void AddCacheService(this IServiceCollection services, IConfiguration _config)
        {
            services.Configure<RedisSettings>(_config.GetSection("RedisSettings"));
            services.AddScoped(sp => sp.GetService<IOptionsSnapshot<RedisSettings>>().Value);
            services.AddTransient<IRedisConnectionWrapper, RedisConnectionWrapper>();
            services.AddTransient<IRedisCacheManager, RedisCacheManager>();

            services.AddMemoryCache();
            services.AddSingleton<IMemoryCacheManager, MemoryCacheManager>();
        }
    }
}
