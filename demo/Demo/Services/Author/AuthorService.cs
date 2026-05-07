namespace Demo.Services.Author;

using System.Linq.Expressions;
using Data;
using Microsoft.EntityFrameworkCore;
using Models.Data;
using Models.Service;

public sealed class AuthorService(
    LibraryDbContext data) : IAuthorService
{
    public async Task<IEnumerable<AuthorServiceModel>> GetAll(
        CancellationToken cancellationToken = default)
        => await data
            .Authors
            .Select(ToServiceModels())
            .ToListAsync(cancellationToken);

    public async Task<AuthorServiceModel?> GetById(
        int id,
        CancellationToken cancellationToken = default)
        => await data
            .Authors
            .Select(ToServiceModels())
            .FirstOrDefaultAsync(
                a => a.Id == id,
                cancellationToken);

    private static Expression<Func<Author, AuthorServiceModel>> ToServiceModels()
        => a => new(
            Id: a.Id,
            Name: a.Name,
            Books: a
                .Books
                .Select(b => new BookServiceModel(
                    Id: b.Id,
                    Title: b.Title,
                    Year: b.Year,
                    IsBorrowed: b.IsBorrowed))
                .ToList());
}