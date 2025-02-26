using Microsoft.AspNetCore.Mvc;
using WatchThisShit.Application.Common.Interfaces.Services;
using WatchThisShit.Application.Common.Models.Requests;
using WatchThisShit.Application.Common.Models.Responses;
using WatchThisShit.Application.Movies.Commands.CreateMovie;
using WatchThisShit.Application.Movies.Commands.DeleteMovie;
using WatchThisShit.Application.Movies.Queries.GetMovieById;
using WatchThisShit.Application.Movies.Queries.GetMovieBySlug;
using WatchThisShit.Application.Movies.Queries.GetMovies;
using WatchThisShit.Web.Endpoints;

namespace WatchThisShit.Web.Controllers;

public class MoviesController : ControllerBase
{
    private readonly ILinkGeneratorService _linkGenerator;
    private readonly IMediator _mediator;


    public MoviesController(ILinkGeneratorService linkGenerator, IMediator mediator)
    {
        _linkGenerator = linkGenerator;
        _mediator = mediator;
    }

    [HttpPost(ApiEndpoints.Movies.Create)]
    public async Task<IActionResult> AddMovie([FromBody] CreateMovieCommand request,
        CancellationToken cancellationToken)
    {
        MovieResponse? response = await _mediator.Send(request, cancellationToken);
        if (response is null)
        {
            return BadRequest();
        }

        return CreatedAtAction(nameof(GetMovie), new { idOrSlug = response.Id }, response);
    }

    // [HttpPut(ApiEndpoints.Movies.Update)]
    // public async Task<IActionResult> UpdateMovie([FromRoute] Guid id, [FromBody] UpdateMovieRequest request,
    //     CancellationToken cancellationToken)
    // {
    //     var movie = request.ToMovieResponse(id);
    //     var updated = await movieService.UpdateAsync(movie, cancellationToken);
    //     if (updated is null)
    //         return BadRequest();
    //     return Ok(updated.ToMovieResponse());
    // }

    [HttpGet(ApiEndpoints.Movies.Get)]
    public async Task<IActionResult> GetMovie([FromRoute] string idOrSlug, CancellationToken cancellationToken)
    {
        var movie = Guid.TryParse(idOrSlug, out Guid id)
            ? await _mediator.Send(new GetMovieByIdQuery(id), cancellationToken)
            : await _mediator.Send(new GetMovieBySlugQuery(idOrSlug), cancellationToken);
        
        if (movie == null)
        {
            return NotFound();
        }

        return Ok(movie);
    }

    [HttpGet(ApiEndpoints.Movies.GetAll)]
    public async Task<IActionResult> GetAllMovies([FromQuery] PaginationFilter filter,
        CancellationToken cancellationToken = default)
    {
        return Ok(await _mediator.Send(new GetMoviesQuery(filter ?? new PaginationFilter()), cancellationToken));
    }

    [HttpDelete(ApiEndpoints.Movies.Delete)]
    public async Task<IActionResult> DeleteMovie([FromRoute] DeleteMovieCommand command,
        CancellationToken cancellationToken)
    {
        bool success = await _mediator.Send(command, cancellationToken);
        if (!success)
        {
            return BadRequest();
        }

        return NoContent();
    }
}
