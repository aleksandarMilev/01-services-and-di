using Exercise.Data;
using Exercise.Services.Movie;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder
    .Services
    .AddOpenApi()
    .AddDbContext<CinemaDbContext>(
        options => options.UseSqlServer(
            builder.Configuration.GetConnectionString("DefaultConnection")))
    .AddScoped<IMovieService, MovieService>()
    .AddControllers();

var app = builder.Build();
var cancellationToken = app.Lifetime.ApplicationStopping;

if (!app.Environment.IsEnvironment("Testing"))
{
    using var scope = app.Services.CreateScope();

    var data = scope
        .ServiceProvider
        .GetRequiredService<CinemaDbContext>();

    await data
        .Database
        .MigrateAsync(cancellationToken);

    await DbSeeder.Seed(
        data,
        cancellationToken);
}

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

if (!app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}

app.UseAuthorization();
app.MapControllers();

await app.RunAsync(cancellationToken);
