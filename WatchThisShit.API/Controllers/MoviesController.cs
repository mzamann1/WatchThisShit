using Microsoft.AspNetCore.Mvc;
using WatchThisShit.API.Mapping;
using WatchThisShit.Application.Services;
using WatchThisShit.Contracts.Requests;

namespace WatchThisShit.API.Controllers;

[ApiController]
public class MoviesController(IMovieService movieService) : ControllerBase
{
    [HttpPost(ApiEndpoints.Movies.Create)]
    public async Task<IActionResult> AddMovie([FromBody] CreateMovieRequest request,
        CancellationToken cancellationToken)
    {
        var movie = request.ToMovie();
        await movieService.CreateAsync(movie, cancellationToken);
        return CreatedAtAction(nameof(GetMovie), new { idOrSlug = movie.Id }, movie);
    }

    [HttpPut(ApiEndpoints.Movies.Update)]
    public async Task<IActionResult> UpdateMovie([FromRoute] Guid id, [FromBody] UpdateMovieRequest request,
        CancellationToken cancellationToken)
    {
        var movie = request.ToMovieResponse(id);
        var updated = await movieService.UpdateAsync(movie, cancellationToken);
        if (updated is null)
            return BadRequest();
        return Ok(updated.ToMovieResponse());
    }

    [HttpGet(ApiEndpoints.Movies.Get)]
    public async Task<IActionResult> GetMovie([FromRoute] string idOrSlug, CancellationToken cancellationToken)
    {
        var movie = Guid.TryParse(idOrSlug, out Guid id)
            ? await movieService.GetByIdAsync(id, cancellationToken)
            : await movieService.GetBySlugAsync(idOrSlug, cancellationToken);
        if (movie == null)
            return NotFound();
        return Ok(movie.ToMovieResponse());
    }

    [HttpGet(ApiEndpoints.Movies.GetAll)]
    public async Task<IActionResult> GetAllMovies(CancellationToken cancellationToken)
    {
        var movies = await movieService.GetAllAsync(cancellationToken);
        return Ok(movies.ToMovieResponse());
    }

    [HttpDelete(ApiEndpoints.Movies.Delete)]
    public async Task<IActionResult> DeleteMovie([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var success = await movieService.DeleteAsync(id, cancellationToken);
        if (!success)
            return BadRequest();
        return Ok();
    }
}