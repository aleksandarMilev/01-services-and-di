namespace Exercise.Models.Web;

using System.ComponentModel.DataAnnotations;

using static Common.Constants.ValidationConstants;

public sealed class CreateMovieRequestModel
{
    [Range(
        MovieYearMinValue,
        MovieYearMaxValue)]
    public int Year { get; init; } = default!;

    [Required]
    [StringLength(
        MovieTitleMaxLength,
        MinimumLength = MovieTitleMinLength)]
    public string Title { get; init; } = default!;

    [Required]
    [StringLength(
        MovieGenreMaxLength,
        MinimumLength = MovieGenreMinLength)]
    public string Genre { get; init; } = default!;
}
