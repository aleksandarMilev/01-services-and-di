namespace Exercise.Controllers;

using Microsoft.AspNetCore.Mvc;
using Models.Web;

// TODO:
// - Inject the IMovieService (use primary constructor).
// - Register the service in Program.cs with the lifetime that makes sense.
// - Implement each action body by calling the correct method of the service.
// - Change the return type of each endpoint from IActionResult to ActionResult<TYourServiceType>.
// - Don't forget to propagate the cancellationToken to the EF Core finalizing methods.
[ApiController]
[Route("api/[controller]")]
public sealed class MoviesController : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll(
        CancellationToken cancellationToken = default)
        => throw new NotImplementedException();

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(
        int id,
        CancellationToken cancellationToken = default)
        => throw new NotImplementedException();

    [HttpPost]
    public async Task<IActionResult> Create(
        CreateMovieRequestModel requestModel,
        CancellationToken cancellationToken = default)
        => throw new NotImplementedException();

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(
        int id,
        CancellationToken cancellationToken = default)
        => throw new NotImplementedException();
}
