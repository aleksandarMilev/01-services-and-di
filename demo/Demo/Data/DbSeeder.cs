namespace Demo.Data;

using Microsoft.EntityFrameworkCore;
using Models.Data;

public static class DbSeeder
{
    public static async Task Seed(
        LibraryDbContext data,
        CancellationToken cancellationToken = default)
    {
        var hasAnyData = await HasAnyData(
            data,
            cancellationToken);

        if (hasAnyData)
        {
            return;
        }

        var authors = await SeedAuthors(
            data,
            cancellationToken);

        await SeedBooks(
            data,
            authors,
            cancellationToken);
    }

    private static async Task SeedBooks(
        LibraryDbContext data,
        List<Author> authors,
        CancellationToken cancellationToken = default)
    {
        var books = new List<Book>
        {
            new()
            {
                Title = "1984",
                Year = 1_949,
                IsBorrowed = false,
                AuthorId = authors[0].Id
            },
            new()
            {
                Title = "Animal Farm",
                Year = 1_945,
                IsBorrowed = false,
                AuthorId = authors[0].Id
            },
            new()
            {
                Title = "Pride and Prejudice",
                Year = 1_813,
                IsBorrowed = false,
                AuthorId = authors[1].Id
            },
            new()
            {
                Title = "Crime and Punishment",
                Year = 1_866,
                IsBorrowed = true,
                AuthorId = authors[2].Id
            }
        };

        data.Books.AddRange(books);
        await data.SaveChangesAsync(cancellationToken);
    }

    private static async Task<List<Author>> SeedAuthors(
        LibraryDbContext data,
        CancellationToken cancellationToken = default)
    {
        var authors = new List<Author>
        {
            new()
            {
                Name = "George Orwell",
            },
            new()
            {
                Name = "Jane Austen",
            },
            new()
            {
                Name = "Fyodor Dostoevsky",
            }
        };

        data.Authors.AddRange(authors);
        await data.SaveChangesAsync(cancellationToken);

        return authors;
    }

    private static async Task<bool> HasAnyData(
        LibraryDbContext data,
        CancellationToken cancellationToken = default)
        => await data.Authors.AnyAsync(cancellationToken) ||
           await data.Books.AnyAsync(cancellationToken);
}
