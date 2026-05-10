namespace Exercise.Services.Movie;

using System.Linq.Expressions;
using Data;
using Microsoft.EntityFrameworkCore;
using Models.Data;
using Models.Service;
using Models.Web;

public sealed class MovieService(CinemaDbContext data) : IMovieService
{
    public async Task<IEnumerable<MovieServiceModel>> GetAll(
        CancellationToken cancellationToken = default)
        => await data
            .Movies
            .Select(ToServiceModel())
            .ToListAsync(cancellationToken);

    public async Task<MovieServiceModel?> GetById(
        int id,
        CancellationToken cancellationToken = default)
        => await data
            .Movies
            .Where(m => m.Id == id)
            .Select(ToServiceModel())
            .FirstOrDefaultAsync(cancellationToken);

    public async Task<MovieServiceModel> Create(
        string title,
        string genre,
        int year,
        CancellationToken cancellationToken = default)
    {
        var movie = new MovieDataModel
        {
            Title = title,
            Genre = genre,
            Year = year,
            IsAvailable = true
        };

        data.Add(movie);
        await data.SaveChangesAsync(cancellationToken);

        return await data
            .Movies
            .Where(m => m.Id == movie.Id)
            .Select(ToServiceModel())
            .FirstAsync(cancellationToken);
    }

    public async Task<bool> Delete(
           int id,
           CancellationToken cancellationToken = default)
    {
        var rowsUpdated = await data
            .Movies
            .Where(m => m.Id == id)
            .ExecuteDeleteAsync(cancellationToken);

        if (rowsUpdated == 0)
        {
            return false;
        }

        return true;
    }

    private static Expression<Func<MovieDataModel, MovieServiceModel>> ToServiceModel()
        => dataModel => new(
            Id: dataModel.Id,
            Title: dataModel.Title,
            Year: dataModel.Year,
            Genre: dataModel.Genre,
            IsAvailable: dataModel.IsAvailable);
}
