namespace Exercise.Models.Service;

public sealed record MovieServiceModel(
    int Id,
    string Title,
    int Year,
    string Genre,
    bool IsAvailable);
