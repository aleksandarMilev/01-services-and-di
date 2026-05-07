using Exercise.Data;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder
    .Services
    .AddOpenApi()
    //.RegisterDbContext()
    //.RegisterIMovieService()
    .AddControllers();

var app = builder.Build();
var cancellationToken = app.Lifetime.ApplicationStopping;

if (!app.Environment.IsEnvironment("Testing"))
{
    // Note: this will crash since the CinemaDbContext is not registrated
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
