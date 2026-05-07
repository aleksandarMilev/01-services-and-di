namespace Demo.Services.Author;

using Models.Service;

public interface IAuthorService
{
    Task<IEnumerable<AuthorServiceModel>> GetAll(
        CancellationToken cancellationToken);

    Task<AuthorServiceModel?> GetById(
        int id,
        CancellationToken cancellationToken);
}