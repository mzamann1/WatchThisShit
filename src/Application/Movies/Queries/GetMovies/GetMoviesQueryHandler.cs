using WatchThisShit.Application.Common.Interfaces.Services;
using WatchThisShit.Application.Common.Models;
using WatchThisShit.Application.Common.Models.Responses;

namespace WatchThisShit.Application.Movies.Queries.GetMovies;

public class GetMoviesQueryHandler : IRequestHandler<GetMoviesQuery, PaginatedList<MovieResponse>>
{
    private readonly IMovieService _movieService;

    public GetMoviesQueryHandler(IMovieService movieService)
    {
        _movieService = movieService;
    }

    public async Task<PaginatedList<MovieResponse>> Handle(GetMoviesQuery request, CancellationToken cancellationToken)
    {
        return await _movieService.GetAllAsync(request.filter, cancellationToken);
    }
}
