using WatchThisShit.Application.Common.Interfaces.Services;

namespace WatchThisShit.Application.Movies.Commands.DeleteMovie;

public class DeleteMovieCommandHandler : IRequestHandler<DeleteMovieCommand, bool>
{
    private readonly IMovieService _movieService;

    public DeleteMovieCommandHandler(IMovieService movieService)
    {
        _movieService = movieService;
    }

    public async Task<bool> Handle(DeleteMovieCommand request, CancellationToken cancellationToken)
    {
        return await _movieService.DeleteAsync(request.Id, cancellationToken);
    }
}
