using System;
using System.Collections.Generic;
using System.Linq;
using WatchThisShit.Application.Models;
using WatchThisShit.Contracts.Requests;
using WatchThisShit.Contracts.Responses;

namespace WatchThisShit.API.Mapping;

public static class ContractMapping
{
    public static Movie ToMovie(this CreateMovieRequest request)
    {
        return new Movie
        {
            Id = Guid.NewGuid(),
            Title = request.Title,
            YearOfRelease = request.YearOfRelease,
            Genres = request.Genres.ToList(),
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
            Slug = movie.Slug,
        };
    }

    public static MoviesResponse ToMovieResponse(this IEnumerable<Movie> movies)
    {
        return new MoviesResponse
        {
            Items = movies.Select(ToMovieResponse)
        };
    }

    public static Movie ToMovieResponse(this UpdateMovieRequest request, Guid id)
    {
        return new Movie
        {
            Id = id,
            Title = request.Title,
            YearOfRelease = request.YearOfRelease,
            Genres = request.Genres.ToList(),
        };
    }
}