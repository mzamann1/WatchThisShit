using WatchThisShit.Application.Common.Interfaces.Services;
using WatchThisShit.Application.Common.Models.Responses;

namespace WatchThisShit.Application.Movies.Queries.GetMovieBySlug;

public class GetMovieBySlugQueryHandler : IRequestHandler<GetMovieBySlugQuery, MovieResponse?>
{
    private readonly IMovieService _movieService;

    public GetMovieBySlugQueryHandler(IMovieService movieService)
    {
        _movieService = movieService;
    }

    public async Task<MovieResponse?> Handle(GetMovieBySlugQuery request, CancellationToken cancellationToken)
    {
        return await _movieService.GetBySlugAsync(request.Slug, cancellationToken);
    }
}
