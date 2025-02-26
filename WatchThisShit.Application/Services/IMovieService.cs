using WatchThisShit.Application.FilterModels;
using WatchThisShit.Application.Models;

namespace WatchThisShit.Application.Services;

public interface IMovieService
{
    Task<bool> CreateAsync(Movie movie, CancellationToken cancellationToken = default);
    Task<(IEnumerable<Movie>, int TotalCount, int PageNumber, int PageSize)> GetAllAsync(PaginationFilter pagination, SortingFilter sorting, CancellationToken cancellationToken = default);
    Task<Movie?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Movie?> GetBySlugAsync(string slug, CancellationToken cancellationToken = default);
    Task<Movie?> UpdateAsync(Movie movie, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}