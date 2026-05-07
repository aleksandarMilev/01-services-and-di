namespace Demo.Controllers;

using Data;
using Demo.Models.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("api/[controller]")]
public sealed class BooksController : ControllerBase
{
    private readonly LibraryDbContext data;

    public BooksController()
    {
        var options = new DbContextOptionsBuilder<LibraryDbContext>()
            .UseSqlServer("Server=sqlserver,1433;Database=Library;User Id=sa;Password=!Passw0rd;TrustServerCertificate=True;")
            .Options;

        this.data = new(options);
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Book>>> GetAll(
        CancellationToken cancellationToken = default)
    {
        var books = await this.data
            .Books
            .ToListAsync(cancellationToken);

        return this.Ok(books);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Book>> GetById(
        int id,
        CancellationToken cancellationToken = default)
    {
        var book = await this.data
            .Books
            .FirstOrDefaultAsync(
                b => b.Id == id,
                cancellationToken); 

        if (book is null)
        {
            var error = new { message = $"Book with id {id} was not found!" };
            return this.NotFound(error);
        }

        return this.Ok(book);
    }

    [HttpPost]
    public async Task<ActionResult<Book>> Create(
        Book request,
        CancellationToken cancellationToken = default)
    {
        var authorExists = await this.data
            .Authors
            .AnyAsync(
                a => a.Id == request.AuthorId,
                cancellationToken);

        if (!authorExists)
        {
            var error = new { message = $"Author with id {request.AuthorId} was not found." };
            return this.NotFound(error);
        }

        var normalizedTitle = request
            .Title
            .Trim()
            .ToLowerInvariant();

        var duplicateBook = await this.data
            .Books
            .AnyAsync(
                b =>
                    b.AuthorId == request.AuthorId &&
                    b.Title.ToLowerInvariant() == normalizedTitle,
                cancellationToken);

        if (duplicateBook)
        {
            var error = new { message = "This author already has a book with the same title." };
            return this.BadRequest(error);
        }

        var book = new Book
        {
            Title = normalizedTitle,
            Year = request.Year,
            AuthorId = request.AuthorId,
            IsBorrowed = false
        };

        this.data.Books.Add(book);
        await this.data.SaveChangesAsync(cancellationToken);

        var createdBook = await this.data
            .Books
            .FirstAsync(
                b => b.Id == book.Id,
                cancellationToken);

        return this.CreatedAtAction(
            nameof(this.GetById),
            new { id = createdBook.Id },
            createdBook);
    }

    [HttpPost("{id:int}/borrow")]
    public async Task<ActionResult<Book>> Borrow(
        int id,
        CancellationToken cancellationToken = default)
    {
        var book = await this.data
            .Books
            .FirstOrDefaultAsync(
                b => b.Id == id,
                cancellationToken);

        if (book is null)
        {
            var error = new { message = $"Book with id {id} was not found." };
            return this.NotFound(error);
        }

        if (book.IsBorrowed)
        {
            var error = new { message = "Book is already borrowed." };
            return this.BadRequest(error);
        }

        book.IsBorrowed = true;
        await this.data.SaveChangesAsync(cancellationToken);

        return this.Ok(book);
    }

    [HttpPost("{id:int}/return")]
    public async Task<ActionResult<Book>> Return(
        int id,
        CancellationToken cancellationToken = default)
    {
        var book = await this.data
            .Books
            .FirstOrDefaultAsync(
                b => b.Id == id,
                cancellationToken);

        if (book is null)
        {
            var error = new { message = $"Book with id {id} was not found." };
            return this.NotFound(error);
        }

        if (!book.IsBorrowed)
        {
            var error = new { message = "Book is not borrowed." };
            return this.BadRequest(error);
        }

        book.IsBorrowed = false;
        await this.data.SaveChangesAsync(cancellationToken);

        return this.Ok(book);
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult> Delete(
        int id,
        CancellationToken cancellationToken = default)
    {
        var book = await this.data
            .Books
            .FirstOrDefaultAsync(
                b => b.Id == id,
                cancellationToken);

        if (book is null)
        {
            var error = new { message = $"Book with id {id} was not found." };
            return this.NotFound(error);
        }

        if (book.IsBorrowed)
        {
            var error = new { message = "Cannot delete a book that is currently borrowed." };
            return this.BadRequest(error);
        }

        this.data.Books.Remove(book);
        await this.data.SaveChangesAsync(cancellationToken);

        return this.NoContent();
    }
}
