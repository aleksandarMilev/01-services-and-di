namespace Exercise.Tests;

using Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

public sealed class ExerciseWebApplicationFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
        => builder
            .UseEnvironment("Testing")
            .ConfigureLogging(static logging => logging.ClearProviders())
            .ConfigureServices(static services =>
            {
                services
                    .Where(static descriptor =>
                        descriptor.ServiceType.FullName is not null &&
                        (descriptor.ServiceType == typeof(CinemaDbContext) ||
                         descriptor.ServiceType.FullName.Contains("DbContext") ||
                         descriptor.ServiceType.FullName.Contains("SqlServer") ||
                         descriptor.ServiceType.FullName.Contains("Relational") ||
                         (descriptor.ServiceType.IsGenericType &&
                          descriptor
                            .ServiceType
                            .GetGenericArguments()
                            .Any(static arg => arg == typeof(CinemaDbContext)))))
                    .ToList()
                    .ForEach(descriptor => services.Remove(descriptor));

                services.AddDbContext<CinemaDbContext>(static options =>
                    options
                        .UseSqlite("DataSource=:memory:")
                        .ConfigureWarnings(static warning =>
                            warning.Ignore(RelationalEventId.PendingModelChangesWarning)));

                var serviceProvider = services.BuildServiceProvider();
                using var scope = serviceProvider.CreateScope();

                var data = scope
                    .ServiceProvider
                    .GetRequiredService<CinemaDbContext>();

                data.Database.OpenConnection();
                data.Database.EnsureCreated();
            });
}
