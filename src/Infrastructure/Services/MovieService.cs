using WatchThisShit.Application.Common.Interfaces.Repositories;
using WatchThisShit.Application.Common.Interfaces.Services;
using WatchThisShit.Application.Common.Mappings;
using WatchThisShit.Application.Common.Models;
using WatchThisShit.Application.Common.Models.Requests;
using WatchThisShit.Application.Common.Models.Responses;
using WatchThisShit.Application.Movies.Commands.CreateMovie;
using WatchThisShit.Domain.Entities;

namespace WatchThisShit.Infrastructure.Services;

public class MovieService : IMovieService
{
    private readonly IMovieRepository _movieRepository;

    public MovieService(IMovieRepository movieRepository)
    {
        _movieRepository = movieRepository;
    }

    public async Task<Movie?> CreateAsync(CreateMovieCommand movie, CancellationToken cancellationToken = default)
    {
        Movie mappedMovie = movie.ToDomainMovie();
        bool success = await _movieRepository.CreateAsync(mappedMovie, cancellationToken);
        return !success ? null : mappedMovie;
    }

    public async Task<PaginatedList<MovieResponse>> GetAllAsync(
        PaginationFilter pagination,
        CancellationToken cancellationToken = default)
    {
        PaginatedList<Movie> movies = await _movieRepository.GetAllAsync(pagination, cancellationToken);
        return movies.MapToMoviesResponsePaginatedList();
    }

    public async Task<MovieResponse?> GetByIdAsync(Guid id,
        CancellationToken cancellationToken = default)
    {
        Movie? movie = await _movieRepository.GetByIdAsync(id, cancellationToken);
        return movie?.ToMovieResponse();
    }

    public async Task<MovieResponse?> GetBySlugAsync(string slug,
        CancellationToken cancellationToken = default)
    {
        Movie? movie = await _movieRepository.GetBySlugAsync(slug, cancellationToken);
        return movie?.ToMovieResponse();
    }

    public async Task<Movie?> UpdateAsync(Movie movie,
        CancellationToken cancellationToken = default)
    {
        // await _validator.ValidateAndThrowAsync(movie, cancellationToken);
        bool exists = await _movieRepository.ExistsByIdAsync(movie.Id, cancellationToken);
        if (!exists)
        {
            return null;
        }

        await _movieRepository.UpdateAsync(movie, cancellationToken);
        return movie;
    }

    public Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return _movieRepository.DeleteAsync(id, cancellationToken);
    }
}
