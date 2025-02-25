using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WatchThisShit.Application.Models;

namespace WatchThisShit.Application.Repositories;

public interface IMovieRepository
{
    Task<bool> CreateAsync(Movie movie);
    Task<IEnumerable<Movie>> GetAllAsync();
    Task<Movie?> GetMovieByIdAsync(Guid id);
    Task<Movie?> GetMovieBySlugAsync(string slug);
    Task<bool> UpdateAsync(Movie movie);
    Task<bool> DeleteAsync(Guid id);
}