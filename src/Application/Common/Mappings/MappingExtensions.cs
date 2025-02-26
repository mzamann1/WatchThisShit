using WatchThisShit.Application.Common.Models;
using WatchThisShit.Application.Common.Models.Responses;
using WatchThisShit.Application.Movies.Commands.CreateMovie;
using WatchThisShit.Domain.Entities;

namespace WatchThisShit.Application.Common.Mappings;

public static class MappingExtensions
{
    public static Movie ToDomainMovie(this CreateMovieCommand request)
    {
        return new Movie
        {
            Id = Guid.NewGuid(),
            Title = request.Title,
            YearOfRelease = request.YearOfRelease,
            Genres = request.Genres.ToList()
        };
    }

    public static MovieResponse ToMovieResponse(this Movie movie)
    {
        return new MovieResponse
        {
            Id = movie.Id,
            Title = movie.Title,
            YearOfRelease = movie.YearOfRelease,
            Genres = movie.Genres.ToList(),
            Slug = movie.Slug
        };
    }

    public static PaginatedList<MovieResponse> MapToMoviesResponsePaginatedList(this IEnumerable<Movie> movies,
        int totalCount,
        int pageNumber, int pageSize)
    {
        return new PaginatedList<MovieResponse>
        {
            Items = movies.Select(ToMovieResponse),
            TotalCount = totalCount,
            PageNumber = pageNumber,
            PageSize = pageSize,
            TotalPages = Convert.ToInt32(Math.Ceiling(totalCount / (double)pageSize))
        };
    }

    public static PaginatedList<Movie> MapToMoviesPaginatedList(this IEnumerable<Movie> movies, int totalCount,
        int pageNumber, int pageSize)
    {
        return new PaginatedList<Movie>
        {
            Items = movies,
            TotalCount = totalCount,
            PageNumber = pageNumber,
            PageSize = pageSize,
            TotalPages = Convert.ToInt32(Math.Ceiling(totalCount / (double)pageSize))
        };
    }


    public static PaginatedList<MovieResponse> MapToMoviesResponsePaginatedList(this PaginatedList<Movie> movies)
    {
        return new PaginatedList<MovieResponse>
        {
            Items = movies.Items.Select(ToMovieResponse),
            TotalCount = movies.TotalCount,
            PageNumber = movies.PageNumber,
            PageSize = movies.PageSize,
            TotalPages = movies.TotalPages
        };
    }
}
