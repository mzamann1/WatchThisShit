using WatchThisShit.Application.Common.Interfaces.Services;
using WatchThisShit.Application.Common.Models.Responses;

namespace WatchThisShit.Application.Movies.Queries.GetMovieById;

public class GetMovieByIdQueryHandler : IRequestHandler<GetMovieByIdQuery, MovieResponse?>
{
    private readonly IMovieService _movieService;

    public GetMovieByIdQueryHandler(IMovieService movieService)
    {
        _movieService = movieService;
    }

    public async Task<MovieResponse?> Handle(GetMovieByIdQuery request, CancellationToken cancellationToken)
    {
        return await _movieService.GetByIdAsync(request.Id, cancellationToken);
    }
}
