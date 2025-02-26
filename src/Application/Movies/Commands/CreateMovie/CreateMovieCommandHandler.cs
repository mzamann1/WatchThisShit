using WatchThisShit.Application.Common.Interfaces.Services;
using WatchThisShit.Application.Common.Mappings;
using WatchThisShit.Application.Common.Models.Responses;
using WatchThisShit.Domain.Entities;

namespace WatchThisShit.Application.Movies.Commands.CreateMovie;

public class CreateMovieCommandHandler : IRequestHandler<CreateMovieCommand, MovieResponse?>
{
    private readonly IMovieService _movieService;

    public CreateMovieCommandHandler(IMovieService movieService)
    {
        _movieService = movieService;
    }

    public async Task<MovieResponse?> Handle(CreateMovieCommand request, CancellationToken cancellationToken)
    {
        Movie? response = await _movieService.CreateAsync(request, cancellationToken);
        return response?.ToMovieResponse();
    }
}
