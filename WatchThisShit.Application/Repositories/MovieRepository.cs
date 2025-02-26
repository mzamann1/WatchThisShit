using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using WatchThisShit.Application.Database;
using WatchThisShit.Application.Models;

namespace WatchThisShit.Application.Repositories;

public class MovieRepository : IMovieRepository
{
    private readonly IDbConnectionFactory _connectionFactory;

    public MovieRepository(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<bool> CreateAsync(Movie movie)
    {
        using var connection = await _connectionFactory.CreateConnectionAsync();
        using var transaction = connection.BeginTransaction();
        var result = await connection.ExecuteAsync(new CommandDefinition(
            """
             INSERT INTO Movies (id, title,slug,yearOfRelease)
             values (@Id, @Title, @Slug, @YearOfRelease)
            """, movie));

        if (result > 0)
            foreach (var genre in movie.Genres)
            {
                await connection.ExecuteAsync(new CommandDefinition(
                    """
                    insert into Genres (movieId, name)
                    values (@Id, @Name)
                    """, new { Id = movie.Id, Name = genre }));
            }

        transaction.Commit();
        connection.Close();
        return result > 0;
    }

    public async Task<IEnumerable<Movie>> GetAllAsync()
    {
        using var connection = await _connectionFactory.CreateConnectionAsync();
        var result = await connection.QueryAsync(new CommandDefinition("""
                                                                       select m.*, string_agg(g.name,',') as genres
                                                                       from movies m 
                                                                       left join genres g on m.id = g.movieId
                                                                       group by m.id
                                                                       """));

        connection.Close();
        return result.Select(m => new Movie
        {
            Id = m.id,
            Title = m.title,
            YearOfRelease = m.yearofrelease,
            Genres = Enumerable.ToList(m.genres.Split(","))
        });
    }

    public async Task<Movie?> GetByIdAsync(Guid id)
    {
        using var connection = await _connectionFactory.CreateConnectionAsync();
        var movie = await connection.QuerySingleOrDefaultAsync<Movie>(
            new CommandDefinition("""
                                  SELECT * FROM movies WHERE id = @id
                                  """, new { id }));

        if (movie is null)
            return null;

        var genres = await connection.QueryAsync<string>(
            new CommandDefinition("""
                                  SELECT name FROM genres WHERE movieid = @id
                                  """, new { id }));
        connection.Close();

        foreach (var genre in genres)
        {
            movie.Genres.Add(genre);
        }

        return movie;
    }

    public async Task<Movie?> GetBySlugAsync(string slug)
    {
        using var connection = await _connectionFactory.CreateConnectionAsync();
        var movie = await connection.QuerySingleOrDefaultAsync<Movie>(
            new CommandDefinition("""
                                  SELECT * FROM movies WHERE slug = @slug
                                  """, new { slug }));

        if (movie is null)
            return null;

        var genres = await connection.QueryAsync<string>(
            new CommandDefinition("""
                                  SELECT name FROM genres WHERE movieid = @id
                                  """, new { id = movie.Id }));
        connection.Close();

        foreach (var genre in genres)
        {
            movie.Genres.Add(genre);
        }

        return movie;
    }

    public async Task<bool> UpdateAsync(Movie movie)
    {
        using var connection = await _connectionFactory.CreateConnectionAsync();

        using var transaction = connection.BeginTransaction();
        await connection.ExecuteAsync(new CommandDefinition("""
                                                            delete from genres where movieid = @id
                                                            """, new { id = movie.Id }));

        foreach (var genre in movie.Genres)
        {
            await connection.ExecuteAsync(new CommandDefinition("""
                                                                insert into genres (movieid, name)
                                                                values (@MovieId, @Name)
                                                                """, new { MovieId = movie.Id, Name = genre }));
        }

        var result = await connection.ExecuteAsync(new CommandDefinition("""
                                                                         update movies
                                                                         set title = @Title,
                                                                         slug = @Slug,
                                                                         yearofrelease = @YearOfRelease
                                                                         where id = @Id
                                                                         """, movie));

        transaction.Commit();
        connection.Close();
        return result > 0;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        using var connection = await _connectionFactory.CreateConnectionAsync();

        using var transaction = connection.BeginTransaction();

        await connection.ExecuteAsync(new CommandDefinition("""
                                                            delete from genres where movieid = @id
                                                            """, new { id = id }));
        var result = await connection.ExecuteAsync(new CommandDefinition("""
                                                                         delete from movies where id = @id
                                                                         """, new { id }));

        transaction.Commit();
        connection.Close();
        return result > 0;
    }

    public async Task<bool> ExistsByIdAsync(Guid id)
    {
        using var connection = await _connectionFactory.CreateConnectionAsync();

        var exist = await connection.ExecuteScalarAsync<bool>(
            new CommandDefinition("select count(1) from movies where id = @id", new { id }));

        connection.Close();
        return exist;
    }
}