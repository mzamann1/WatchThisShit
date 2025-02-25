using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WatchThisShit.Application.Models;

namespace WatchThisShit.Application.Repositories;

public class MovieRepository : IMovieRepository
{
    
    private readonly List<Movie> _movies= [];
    public Task<bool> CreateAsync(Movie movie)
    {
        _movies.Add(movie);
        return Task.FromResult(true);
    }

    public Task<IEnumerable<Movie>> GetAllAsync()
    {
        return Task.FromResult(_movies.AsEnumerable());
    }

    public Task<Movie?> GetMovieByIdAsync(Guid id)
    {
        return Task.FromResult(_movies.FirstOrDefault(m => m.Id == id));
    }

    public Task<Movie?> GetMovieBySlugAsync(string slug)
    {
        return Task.FromResult(_movies.FirstOrDefault(m => m.Slug == slug));
    }

    public Task<bool> UpdateAsync(Movie movie)
    {
       var movieIndex= _movies.FindIndex(x=>x.Id == movie.Id);

       if (movieIndex==-1)
       {
           return Task.FromResult(false);
       }
       
       _movies[movieIndex] = movie;
       return Task.FromResult(true);
    }

    public Task<bool> DeleteAsync(Guid id)
    {
        var removedCount= _movies.RemoveAll(x=>x.Id == id);
        var movieRemoved = removedCount > 0;
        return Task.FromResult(movieRemoved);
    }
}