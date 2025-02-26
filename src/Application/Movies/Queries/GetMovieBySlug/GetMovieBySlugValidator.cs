using WatchThisShit.Application.Common.Models.Responses;

namespace WatchThisShit.Application.Movies.Queries.GetMovieBySlug;

public record GetMovieBySlugQuery(string Slug) : IRequest<MovieResponse?>
{
}
