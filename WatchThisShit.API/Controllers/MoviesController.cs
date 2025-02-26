using Microsoft.AspNetCore.Mvc;
using WatchThisShit.API.Mapping;
using WatchThisShit.API.Services;
using WatchThisShit.Application.FilterModels;
using WatchThisShit.Application.Services;
using WatchThisShit.Contracts.Common;
using WatchThisShit.Contracts.Requests;
using WatchThisShit.Contracts.Responses;

namespace WatchThisShit.API.Controllers;

[ApiController]
public class MoviesController(IMovieService movieService, ILinkGeneratorService linkGenerator) : ControllerBase
{
    private readonly Guid _userId = Guid.Parse("1d21fb92-5348-42ef-8bca-3743baea12bb");

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
    public async Task<IActionResult> GetAllMovies([FromQuery] int pageNumber = 1,
    [FromQuery] int pageSize = 10,
    [FromQuery] string? sortBy = null,
    [FromQuery] bool isDescending = false,
    CancellationToken cancellationToken = default)
    {
        var pagination = new PaginationFilter() { PageNumber = pageNumber, PageSize = pageSize };
        var sorting = new SortingFilter() { SortBy = sortBy, IsDescending = isDescending };
        var (movies, totalCount, pageNumberRet, pageSizeRet) = await movieService.GetAllAsync(pagination, sorting, cancellationToken);
        return Ok(new PagedResult<MovieResponse>
        {
            Items = movies.Select(m => {
                var movie = m.ToMovieResponse();
                movie.Links = linkGenerator.GenerateMovieLinks(movie.Id, movie.Slug);
                return movie;
            }),
            TotalCount = totalCount,
            PageNumber = pageNumberRet,
            PageSize = pageSizeRet,
            TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize)
        });
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