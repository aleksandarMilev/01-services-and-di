namespace Demo.Models.Data;

public sealed class Author
{
    public int Id { get; init; }

    public string Name { get; init; } = default!;

    public List<Book> Books { get; init; } = [];
}
