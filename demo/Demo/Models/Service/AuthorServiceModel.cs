namespace Demo.Models.Service;

public sealed record BookServiceModel(
    int Id,
    string Title,
    int Year,
    bool IsBorrowed);

public sealed record AuthorServiceModel(
    int Id,
    string Name,
    IReadOnlyList<BookServiceModel> Books);
