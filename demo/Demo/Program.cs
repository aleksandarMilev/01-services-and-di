using Demo.Data;
using Demo.Services.Author;
using Demo.Services.LifetimeDemo.Scoped;
using Demo.Services.LifetimeDemo.Singleton;
using Demo.Services.LifetimeDemo.Transient;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder
    .Services
    .AddOpenApi()
    .AddDbContext<LibraryDbContext>(options =>
        options.UseSqlServer(
            builder.Configuration.GetConnectionString("DefaultConnection")))
    .AddSingleton<ISingletonService, SingletonService>()
    .AddScoped<IScopedService, ScopedService>()
    .AddTransient<ITransientService, TransientService>()
    .AddScoped<IAuthorService, AuthorService>()
    .AddControllers();

var app = builder.Build();
var cancelationToken = app.Lifetime.ApplicationStopping;

using (var scope = app.Services.CreateScope())
{
    var data = scope
        .ServiceProvider
        .GetRequiredService<LibraryDbContext>();

    await data
        .Database
        .MigrateAsync(cancelationToken);

    await DbSeeder.Seed(
        data,
        cancelationToken);
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

await app.RunAsync(cancelationToken);
