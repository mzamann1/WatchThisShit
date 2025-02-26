using System.Data;
using Dapper;
using WatchThisShit.Application.Common.Interfaces;
using WatchThisShit.Application.Common.Interfaces.Repositories;
using WatchThisShit.Application.Common.Mappings;
using WatchThisShit.Application.Common.Models;
using WatchThisShit.Application.Common.Models.Requests;
using WatchThisShit.Domain.Entities;

namespace WatchThisShit.Infrastructure.Repositories;

public class MovieRepository(IDbConnectionFactory connectionFactory) : IMovieRepository
{
    public async Task<bool> CreateAsync(Movie movie, CancellationToken cancellationToken = default)
    {
        using IDbConnection connection = await connectionFactory.CreateConnectionAsync(cancellationToken);
        using IDbTransaction transaction = connection.BeginTransaction();
        int result = await connection.ExecuteAsync(new CommandDefinition(
            """
             INSERT INTO Movies (id, title, slug, yearofrelease)
             values (@Id, @Title, @Slug, @YearOfRelease)
            """, movie, transaction, cancellationToken: cancellationToken));

        if (result > 0)
        {
            foreach (string genre in movie.Genres)
            {
                await connection.ExecuteAsync(new CommandDefinition(
                    """
                    insert into Genres (movieId, name)
                    values (@Id, @Name)
                    """, new { movie.Id, Name = genre }, transaction,
                    cancellationToken: cancellationToken));
            }
        }

        transaction.Commit();
        connection.Close();
        return result > 0;
    }

    public async Task<PaginatedList<Movie>> GetAllAsync(PaginationFilter pagination,
        CancellationToken cancellationToken = default)
    {
        using IDbConnection connection = await connectionFactory.CreateConnectionAsync(cancellationToken);

        string orderBy = pagination.OrderBy?.ToLower() switch
        {
            "title" => "title",
            "year" => "yearofrelease",
            _ => "title"
        };

        string direction = pagination.IsDescending ? "DESC" : "ASC";
        int offset = (pagination.PageNumber - 1) * pagination.PageSize;

        string sql = $"""
                      WITH MovieData AS (
                          SELECT m.*, string_agg(g.name, ',') as genres,
                                 COUNT(*) OVER() as TotalCount
                          FROM movies m 
                          LEFT JOIN genres g ON m.id = g.movieid
                          GROUP BY m.id
                      )
                      SELECT *
                      FROM MovieData
                      ORDER BY {orderBy} {direction}
                      LIMIT @PageSize OFFSET @Offset
                      """;

        IEnumerable<dynamic> result = await connection.QueryAsync(new CommandDefinition(sql,
            new { pagination.PageSize, Offset = offset }, cancellationToken: cancellationToken));
        int totalCount = (int)(result.FirstOrDefault()?.totalcount ?? 0);
        connection.Close();

        IEnumerable<Movie> movies = result.Select(m => new Movie
        {
            Id = m.id,
            Title = m.title,
            YearOfRelease = m.yearofrelease,
            Genres = Enumerable.ToList(m.genres?.Split(",", StringSplitOptions.RemoveEmptyEntries)) ??
                     Enumerable.Empty<string>()
        });

        return movies.MapToMoviesPaginatedList(totalCount, pagination.PageNumber, pagination.PageSize);
    }

    public async Task<Movie?> GetByIdAsync(Guid id,
        CancellationToken cancellationToken = default)
    {
        using IDbConnection connection = await connectionFactory.CreateConnectionAsync(cancellationToken);
        Movie? movie = await connection.QuerySingleOrDefaultAsync<Movie>(
            new CommandDefinition("""
                                  SELECT * FROM movies WHERE id = @id
                                  """, new { id }, cancellationToken: cancellationToken));

        if (movie is null)
        {
            return null;
        }

        IEnumerable<string> genres = await connection.QueryAsync<string>(
            new CommandDefinition("""
                                  SELECT name FROM genres WHERE movieid = @id
                                  """, new { id }, cancellationToken: cancellationToken));
        connection.Close();

        foreach (string genre in genres)
        {
            movie.Genres.Add(genre);
        }

        return movie;
    }

    public async Task<Movie?> GetBySlugAsync(string slug,
        CancellationToken cancellationToken = default)
    {
        using IDbConnection connection = await connectionFactory.CreateConnectionAsync(cancellationToken);
        Movie? movie = await connection.QuerySingleOrDefaultAsync<Movie>(
            new CommandDefinition("""
                                  SELECT * FROM movies WHERE slug = @slug
                                  """, new { slug }, cancellationToken: cancellationToken));

        if (movie is null)
        {
            return null;
        }

        IEnumerable<string> genres = await connection.QueryAsync<string>(
            new CommandDefinition("""
                                  SELECT name FROM genres WHERE movieid = @id
                                  """, new { id = movie.Id }, cancellationToken: cancellationToken));
        connection.Close();

        foreach (string genre in genres)
        {
            movie.Genres.Add(genre);
        }

        return movie;
    }

    public async Task<bool> UpdateAsync(Movie movie,
        CancellationToken cancellationToken = default)
    {
        using IDbConnection connection = await connectionFactory.CreateConnectionAsync(cancellationToken);

        using IDbTransaction transaction = connection.BeginTransaction();
        await connection.ExecuteAsync(new CommandDefinition("""
                                                            delete from genres where movieid = @id
                                                            """, new { id = movie.Id }, transaction,
            cancellationToken: cancellationToken));

        foreach (string genre in movie.Genres)
        {
            await connection.ExecuteAsync(new CommandDefinition("""
                                                                insert into genres (movieid, name)
                                                                values (@MovieId, @Name)
                                                                """, new { MovieId = movie.Id, Name = genre },
                transaction, cancellationToken: cancellationToken));
        }

        int result = await connection.ExecuteAsync(new CommandDefinition("""
                                                                         update movies
                                                                         set title = @Title,
                                                                         slug = @Slug,
                                                                         yearofrelease = @YearOfRelease
                                                                         where id = @Id
                                                                         """, movie, transaction,
            cancellationToken: cancellationToken));

        transaction.Commit();
        connection.Close();
        return result > 0;
    }

    public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        using IDbConnection connection = await connectionFactory.CreateConnectionAsync(cancellationToken);

        using IDbTransaction transaction = connection.BeginTransaction();

        await connection.ExecuteAsync(new CommandDefinition("""
                                                            delete from genres where movieid = @id
                                                            """, new { id }));
        int result = await connection.ExecuteAsync(new CommandDefinition("""
                                                                         delete from movies where id = @id
                                                                         """, new { id }, transaction,
            cancellationToken: cancellationToken));

        transaction.Commit();
        connection.Close();
        return result > 0;
    }

    public async Task<bool> ExistsByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        using IDbConnection connection = await connectionFactory.CreateConnectionAsync(cancellationToken);

        bool exist = await connection.ExecuteScalarAsync<bool>(
            new CommandDefinition("select count(1) from movies where id = @id", new { id },
                cancellationToken: cancellationToken));

        connection.Close();
        return exist;
    }
}
