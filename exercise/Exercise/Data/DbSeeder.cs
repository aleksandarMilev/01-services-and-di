namespace Exercise.Data;

using Microsoft.EntityFrameworkCore;
using Models.Data;

public static class DbSeeder
{
    public static async Task Seed(
        CinemaDbContext data,
        CancellationToken cancellationToken = default)
    {
        var hasAnyData = await HasAnyData(
            data,
            cancellationToken);

        if (hasAnyData)
        {
            return;
        }

        await SeedMovies(
            data,
            cancellationToken);
    }

    private static async Task SeedMovies(
        CinemaDbContext data,
        CancellationToken cancellationToken = default)
    {
        var movies = new List<MovieDataModel>
        {
            new()
            {
                Title = "The Shawshank Redemption",
                Year = 1_994,
                Genre = "Drama",
                IsAvailable = true
            },
            new()
            {
                Title = "The Godfather",
                Year = 1_972,
                Genre = "Crime",
                IsAvailable = true
            },
            new()
            {
                Title = "The Dark Knight",
                Year = 2_008,
                Genre = "Action",
                IsAvailable = false
            },
            new()
            {
                Title = "Schindler's List",
                Year = 1_993,
                Genre = "History",
                IsAvailable = true
            }
        };

        data.Movies.AddRange(movies);
        await data.SaveChangesAsync(cancellationToken);
    }

    private static async Task<bool> HasAnyData(
        CinemaDbContext data,
        CancellationToken cancellationToken = default)
        => await data.Movies.AnyAsync(cancellationToken);
}