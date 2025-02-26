using WatchThisShit.Application.Common.Models.Responses;

namespace WatchThisShit.Application.Movies.Commands.CreateMovie;

public record CreateMovieCommand : IRequest<MovieResponse?>
{
    public required string Title { get; init; }
    public required int YearOfRelease { get; init; }
    public List<string> Genres { get; init; } = new();
}
