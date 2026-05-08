namespace Exercise.Tests;

using Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

public sealed class ExerciseWebApplicationFactory : WebApplicationFactory<Program>
{
    private readonly SqliteConnection connection = new("DataSource=:memory:");

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        this.connection.Open();

        builder
            .UseEnvironment("Testing")
            .ConfigureLogging(static logging => logging.ClearProviders())
            .ConfigureServices(services =>
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

                services.AddDbContext<CinemaDbContext>(options =>
                    options
                        .UseSqlite(this.connection)
                        .ConfigureWarnings(static warning =>
                            warning.Ignore(RelationalEventId.PendingModelChangesWarning)));

                var serviceProvider = services.BuildServiceProvider();
                using var scope = serviceProvider.CreateScope();

                var data = scope
                    .ServiceProvider
                    .GetRequiredService<CinemaDbContext>();

                data.Database.EnsureCreated();
            });
    }

    protected override void Dispose(bool disposing)
    {
        base.Dispose(disposing);
        this.connection.Dispose();
    }
}