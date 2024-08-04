using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;

namespace Hiwu.SpecificationPattern.SampleApi.Context
{
    /// <summary>
    /// Factory for creating instances of <see cref="AppDbContext"/> at design time.
    /// </summary>
    public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
    {
        public AppDbContext CreateDbContext(string[] args)
        {
            // Get the current environment, default to Production if not set
            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production";

            // Build the configuration by adding the base appsettings.json and environment-specific appsettings
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .AddJsonFile($"appsettings.{environment}.json", optional: true)
                .Build();

            // Retrieve the connection string from the configuration
            var connectionString = configuration.GetConnectionString("ConnectionString");

            // Create options builder and configure it to use SQL Server with the retrieved connection string
            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
            //optionsBuilder.UseSqlServer(connectionString);
            optionsBuilder.UseNpgsql(connectionString);

            // Return a new instance of AppDbContext with the configured options
            return new AppDbContext(optionsBuilder.Options);
        }
    }
}
