using WatchThisShit.Application.Common.Models;
using WatchThisShit.Application.Common.Models.Requests;
using WatchThisShit.Application.Common.Models.Responses;
using WatchThisShit.Application.Movies.Commands.CreateMovie;
using WatchThisShit.Domain.Entities;

namespace WatchThisShit.Application.Common.Interfaces.Services;

public interface IMovieService
{
    Task<Movie?> CreateAsync(CreateMovieCommand movie, CancellationToken cancellationToken = default);

    Task<PaginatedList<MovieResponse>> GetAllAsync(PaginationFilter pagination,
        CancellationToken cancellationToken = default);

    Task<MovieResponse?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<MovieResponse?> GetBySlugAsync(string slug, CancellationToken cancellationToken = default);
    Task<Movie?> UpdateAsync(Movie movie, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}
