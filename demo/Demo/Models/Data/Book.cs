namespace Demo.Models.Data;

public sealed class Book
{
    public int Id { get; init; }

    public string Title { get; init; } = default!;

    public int Year { get; init; }

    public bool IsBorrowed { get; set; }

    public int AuthorId { get; init; }

    public Author Author { get; init; } = default!;
}
