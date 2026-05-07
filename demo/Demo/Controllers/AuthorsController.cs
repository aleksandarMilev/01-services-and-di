namespace Demo.Controllers;

using Microsoft.AspNetCore.Mvc;
using Services.Author;

[ApiController]
[Route("api/[controller]")]
public sealed class AuthorsController(
    IAuthorService service) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll(
        CancellationToken cancellationToken = default)
    {
        var authors = await service.GetAll(cancellationToken);
        return this.Ok(authors);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(
        int id,
        CancellationToken cancellationToken = default)
    {
        var author = await service.GetById(
            id,
            cancellationToken);

        if (author is null)
        {
            var error = new { message = $"Author with id {id} was not found!" };
            return this.NotFound(error);
        }

        return this.Ok(author);
    }
}