namespace WatchThisShit.Application.Movies.Commands.DeleteMovie;

public record DeleteMovieCommand : IRequest<bool>
{
    public required Guid Id { get; init; }
}
