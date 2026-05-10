namespace Exercise.Controllers;

using Microsoft.AspNetCore.Mvc;
using Models.Service;
using Models.Web;
using Services.Movie;

[ApiController]
[Route("api/[controller]")]
public sealed class MoviesController(
    IMovieService service) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<MovieServiceModel>>> GetAll(
        CancellationToken cancellationToken = default)
    {
        var movies = await service.GetAll(cancellationToken);
        return this.Ok(movies);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<MovieServiceModel>> GetById(
        int id,
        CancellationToken cancellationToken = default)
    {
        var movie = await service.GetById(
            id,
            cancellationToken);

        if (movie is null)
        {
            return this.NotFound();
        }

        return this.Ok(movie);
    }

    [HttpPost]
    public async Task<ActionResult<MovieServiceModel>> Create(
        CreateMovieRequestModel requestModel,
        CancellationToken cancellationToken = default)
    {
        var createdMovie = await service.Create(
            requestModel.Title,
            requestModel.Genre,
            requestModel.Year,
            cancellationToken);

        return this.CreatedAtAction(
            nameof(this.GetById),
            new { id = createdMovie.Id },
            createdMovie);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(
        int id,
        CancellationToken cancellationToken = default)
    {
        var deleted = await service.Delete(
            id,
            cancellationToken);

        if (!deleted)
        {
            return this.NotFound();
        }

        return this.NoContent();
    }
}
