using WatchThisShit.Application.Common.Models;
using WatchThisShit.Application.Common.Models.Requests;
using WatchThisShit.Application.Common.Models.Responses;

namespace WatchThisShit.Application.Movies.Queries.GetMovies;

public record GetMoviesQuery(PaginationFilter filter) : IRequest<PaginatedList<MovieResponse>>
{
}
