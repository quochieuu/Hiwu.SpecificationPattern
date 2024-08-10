using Hiwu.SpecificationPattern.Mongo.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;

namespace Hiwu.SpecificationPattern.Mongo
{
    public static class ServiceRegister
    {
        public static void AddMongo(this IServiceCollection services, IConfiguration configuration)
        {
            var options = new MongoDbOptions();
            configuration.GetSection("MongoDb").Bind(options);

            services.AddSingleton<IMongoClient>(sp =>
            {
                return new MongoClient(options.ConnectionString);
            });

            services.AddScoped(typeof(IMongoRepository<>), typeof(MongoRepository<>));

            services.AddScoped<IMongoDatabase>(sp =>
            {
                var client = sp.GetRequiredService<IMongoClient>();
                return client.GetDatabase(options.Database);
            });
        }
    }
}
