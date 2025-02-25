using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WatchThisShit.API.Mapping;
using WatchThisShit.Application.Repositories;
using WatchThisShit.Contracts.Requests;

namespace WatchThisShit.API.Controllers;

[ApiController]
public class MoviesController(IMovieRepository movieRepository) : ControllerBase
{
    [HttpPost(ApiEndpoints.Movies.Create)]
    public async Task<IActionResult> AddMovie([FromBody] CreateMovieRequest request)
    {
        var movie = request.ToMovie();
        await movieRepository.CreateAsync(movie);
        return CreatedAtAction(nameof(GetMovie), new { idOrSlug = movie.Id }, movie);
    }

    [HttpPut(ApiEndpoints.Movies.Update)]
    public async Task<IActionResult> UpdateMovie([FromRoute] Guid id, [FromBody] UpdateMovieRequest request)
    {
        var movie = request.ToMovieResponse(id);
        var updated = await movieRepository.UpdateAsync(movie);
        if (!updated)
            return BadRequest();
        return Ok(movie.ToMovieResponse());
    }

    [HttpGet(ApiEndpoints.Movies.Get)]
    public async Task<IActionResult> GetMovie([FromRoute] string idOrSlug)
    {
        var movie = Guid.TryParse(idOrSlug, out Guid id)
            ? await movieRepository.GetMovieByIdAsync(id)
            : await movieRepository.GetMovieBySlugAsync(idOrSlug);
        if (movie == null)
            return NotFound();
        return Ok(movie.ToMovieResponse());
    }

    [HttpGet(ApiEndpoints.Movies.GetAll)]
    public async Task<IActionResult> GetAllMovies()
    {
        var movies = await movieRepository.GetAllAsync();
        return Ok(movies.ToMovieResponse());
    }

    [HttpDelete(ApiEndpoints.Movies.Delete)]
    public async Task<IActionResult> DeleteMovie([FromRoute] Guid id)
    {
        var success = await movieRepository.DeleteAsync(id);
        if (!success)
            return BadRequest();
        return Ok();
    }
}