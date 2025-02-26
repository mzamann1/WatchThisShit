using WatchThisShit.Application.Common.Models.Responses;

namespace WatchThisShit.Application.Movies.Queries.GetMovieById;

public record GetMovieByIdQuery(Guid Id) : IRequest<MovieResponse?>
{
}
