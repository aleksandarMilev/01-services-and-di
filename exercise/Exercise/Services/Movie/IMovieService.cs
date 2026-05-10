namespace Exercise.Services.Movie;

using Models.Service;

public interface IMovieService
{
    Task<IEnumerable<MovieServiceModel>> GetAll(
        CancellationToken cancellationToken = default);

    Task<MovieServiceModel?> GetById(
        int id,
        CancellationToken cancellationToken = default);

    Task<MovieServiceModel> Create(
        string title,
        string genre,
        int year,
        CancellationToken cancellationToken = default);

    Task<bool> Delete(
        int id,
        CancellationToken cancellationToken = default);
}
