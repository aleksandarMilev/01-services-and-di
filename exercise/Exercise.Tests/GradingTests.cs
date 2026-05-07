namespace Exercise.Tests;

using System.Net;
using System.Text;
using Controllers;
using Data;
using Microsoft.Extensions.DependencyInjection;
using Models.Data;
using Models.Service;
using Models.Web;
using Services.Movie;
using Xunit;
using Xunit.Sdk;

public sealed class GradingTests(
    ExerciseWebApplicationFactory factory) : IClassFixture<ExerciseWebApplicationFactory>
{
    private readonly HttpClient client = factory.CreateClient();

    [Fact(DisplayName = "[2 points] App starts without errors")]
    public void App_ShouldStartWithoutErrors()
    {
        using var scope = factory.Services.CreateScope();
        var serviceProvider = scope.ServiceProvider;

        Verify(
            serviceProvider.GetService<CinemaDbContext>() is not null,
            "CinemaDbContext is not registered in Program.cs — call AddDbContext<CinemaDbContext>(...).");

        Verify(
            serviceProvider.GetService<IMovieService>() is not null,
            "IMovieService is not registered in Program.cs — register it with the appropriate lifetime.");
    }

    [Fact(DisplayName = "[2 points] All MoviesController endpoints are implemented")]
    public async Task MoviesController_ShouldImplementAllEndpoints()
    {
        await AssertEndpointIsImplemented(
            () => this.client.GetAsync("/api/movies"),
            "GET /api/movies — GetAll() is not implemented.");

        await AssertEndpointIsImplemented(
            () => this.client.GetAsync("/api/movies/1"),
            "GET /api/movies/1 — GetById() is not implemented.");

        var postPayload = new StringContent(
            """{"title":"Test Movie","year":2000,"genre":"Drama"}""",
            Encoding.UTF8,
            "application/json");

        await AssertEndpointIsImplemented(
            () => this.client.PostAsync(
                "/api/movies",
                postPayload),
            "POST /api/movies — Create() is not implemented.");

        await AssertEndpointIsImplemented(
            () => this.client.DeleteAsync("/api/movies/1"),
            "DELETE /api/movies/1 — Delete() is not implemented.");
    }

    [Fact(DisplayName = "[2 points] MoviesController does not use CinemaDbContext directly")]
    public void MoviesController_ShouldInject_IMovieService_NotCinemaDbContext()
    {
        var controllerType = typeof(MoviesController);

        Verify(
            !controllerType
                .GetConstructors()
                .Any(static constructor => constructor
                    .GetParameters()
                    .Any(static param => param.ParameterType == typeof(CinemaDbContext))),
            "MoviesController injects CinemaDbContext directly — inject IMovieService instead.");

        Verify(
            controllerType
                .GetConstructors()
                .Any(static constructor => constructor
                    .GetParameters()
                    .Any(static param => param.ParameterType == typeof(IMovieService))),
            "MoviesController does not inject IMovieService — add it via primary constructor.");
    }

    [Fact(DisplayName = "[2 points] MoviesController does not return MovieDataModel")]
    public void MoviesController_ShouldReturn_ServiceModel_NotDataModel()
    {
        var controllerType = typeof(MoviesController);
        var actions = controllerType
            .GetMethods()
            .Where(method =>
                method.IsPublic &&
                !method.IsSpecialName &&
                method.DeclaringType == controllerType);

        Verify(
            !actions.Any(static method => ContainsType(method.ReturnType, typeof(MovieDataModel))),
            "MoviesController returns MovieDataModel directly — return MovieServiceModel instead.");

        Verify(
            actions.Any(static method => ContainsType(method.ReturnType, typeof(MovieServiceModel))),
            "MoviesController does not return MovieServiceModel in any action — " +
            "make sure GetAll or GetById returns it.");
    }

    [Fact(DisplayName = "[2 points] MovieService does not use web models or HTTP-specific types")]
    public void MovieService_ShouldNotUse_WebModels()
    {
        var serviceType = typeof(MovieService);

        static bool IsHttpSpecific(Type type)
            => type == typeof(CreateMovieRequestModel) ||
               type.Namespace?.StartsWith("Microsoft.AspNetCore") == true;

        var publicMethods = serviceType
            .GetMethods()
            .Where(method =>
                method.IsPublic &&
                !method.IsSpecialName
                && method.DeclaringType == serviceType)
            .ToList();

        Verify(
            publicMethods.Count > 0,
            "MovieService has no public methods — implement the service first.");

        Verify(
            !serviceType
                .GetConstructors()
                .Any(static constructor => constructor
                    .GetParameters()
                    .Any(static praram => IsHttpSpecific(praram.ParameterType))),
            "MovieService uses web models or ASP.NET Core types in its constructor — " +
            "services must not be aware of HTTP concerns.");

        Verify(
            !publicMethods.Any(static method => 
                method
                    .GetParameters()
                    .Any(static param => IsHttpSpecific(param.ParameterType))),
            "MovieService uses web models or ASP.NET Core types in its methods — " +
            "services must not be aware of HTTP concerns.");
    }

    private static void Verify(
        bool condition,
        string message)
    {
        if (!condition)
        {
            throw new XunitException(message);
        }
    }

    private static async Task AssertEndpointIsImplemented(
        Func<Task<HttpResponseMessage>> call,
        string failMessage)
    {
        try
        {
            var response = await call();
            if (response.StatusCode == HttpStatusCode.InternalServerError)
            {
                throw new XunitException($"{failMessage} (returned 500)");
            }
        }
        catch (NotImplementedException)
        {
            throw new XunitException(failMessage);
        }
    }

    private static bool ContainsType(
        Type returnType,
        Type target)
    {
        if (returnType == target)
        {
            return true;
        }

        if (!returnType.IsGenericType)
        {
            return false;
        }

        return returnType
            .GetGenericArguments()
            .Any(arg => ContainsType(arg, target));
    }
}
