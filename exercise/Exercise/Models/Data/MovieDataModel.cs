namespace Exercise.Models.Data;

public sealed class MovieDataModel
{
    public int Id { get; init; }

    public string Title { get; init; } = default!;

    public int Year { get; init; }

    public string Genre { get; init; } = default!;

    public bool IsAvailable { get; init; }
}
