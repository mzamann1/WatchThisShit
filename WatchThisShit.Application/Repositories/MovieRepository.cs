using Dapper;
using WatchThisShit.Application.Database;
using WatchThisShit.Application.Models;
using System.Linq;
using WatchThisShit.Application.FilterModels;

namespace WatchThisShit.Application.Repositories;

public class MovieRepository(IDbConnectionFactory connectionFactory) : IMovieRepository
{
    public async Task<bool> CreateAsync(Movie movie, CancellationToken cancellationToken = default)
    {
        using var connection = await connectionFactory.CreateConnectionAsync(cancellationToken);
        using var transaction = connection.BeginTransaction();
        var result = await connection.ExecuteAsync(new CommandDefinition(
            """
             INSERT INTO Movies (id, title, slug, yearofrelease)
             values (@Id, @Title, @Slug, @YearOfRelease)
            """, movie, transaction: transaction, cancellationToken: cancellationToken));

        if (result > 0)
            foreach (var genre in movie.Genres)
            {
                await connection.ExecuteAsync(new CommandDefinition(
                    """
                    insert into Genres (movieId, name)
                    values (@Id, @Name)
                    """, new { Id = movie.Id, Name = genre }, transaction: transaction,
                    cancellationToken: cancellationToken));
            }

        transaction.Commit();
        connection.Close();
        return result > 0;
    }

    public async Task<(IEnumerable<Movie>, int, int, int)> GetAllAsync(PaginationFilter pagination,
        SortingFilter sorting,
        CancellationToken cancellationToken = default)
    {
        using var connection = await connectionFactory.CreateConnectionAsync(cancellationToken);

        var orderBy = sorting.SortBy?.ToLower() switch
        {
            "title" => "title",
            "year" => "yearofrelease",
            _ => "title"
        };

        var direction = sorting.IsDescending ? "DESC" : "ASC";
        var offset = (pagination.PageNumber - 1) * pagination.PageSize;

        var sql = $"""
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

        var result = await connection.QueryAsync(new CommandDefinition(sql,
            new { PageSize = pagination.PageSize, Offset = offset }, cancellationToken: cancellationToken));
        var totalCount = (int) (result.FirstOrDefault()?.totalcount ?? 0);
        connection.Close();

        var movies = result.Select(m => new Movie
        {
            Id = m.id,
            Title = m.title,
            YearOfRelease = m.yearofrelease,
            Genres = Enumerable.ToList(m.genres?.Split(",", StringSplitOptions.RemoveEmptyEntries)) ?? Enumerable.Empty<string>()
        });

        return (movies, totalCount, pagination.PageNumber, pagination.PageSize);
    }

    public async Task<Movie?> GetByIdAsync(Guid id,
        CancellationToken cancellationToken = default)
    {
        using var connection = await connectionFactory.CreateConnectionAsync(cancellationToken);
        var movie = await connection.QuerySingleOrDefaultAsync<Movie>(
            new CommandDefinition("""
                                  SELECT * FROM movies WHERE id = @id
                                  """, new { id }, cancellationToken: cancellationToken));

        if (movie is null)
            return null;

        var genres = await connection.QueryAsync<string>(
            new CommandDefinition("""
                                  SELECT name FROM genres WHERE movieid = @id
                                  """, new { id }, cancellationToken: cancellationToken));
        connection.Close();

        foreach (var genre in genres)
        {
            movie.Genres.Add(genre);
        }

        return movie;
    }

    public async Task<Movie?> GetBySlugAsync(string slug,
        CancellationToken cancellationToken = default)
    {
        using var connection = await connectionFactory.CreateConnectionAsync(cancellationToken);
        var movie = await connection.QuerySingleOrDefaultAsync<Movie>(
            new CommandDefinition("""
                                  SELECT * FROM movies WHERE slug = @slug
                                  """, new { slug }, cancellationToken: cancellationToken));

        if (movie is null)
            return null;

        var genres = await connection.QueryAsync<string>(
            new CommandDefinition("""
                                  SELECT name FROM genres WHERE movieid = @id
                                  """, new { id = movie.Id }, cancellationToken: cancellationToken));
        connection.Close();

        foreach (var genre in genres)
        {
            movie.Genres.Add(genre);
        }

        return movie;
    }

    public async Task<bool> UpdateAsync(Movie movie,
        CancellationToken cancellationToken = default)
    {
        using var connection = await connectionFactory.CreateConnectionAsync(cancellationToken);

        using var transaction = connection.BeginTransaction();
        await connection.ExecuteAsync(new CommandDefinition("""
                                                            delete from genres where movieid = @id
                                                            """, new { id = movie.Id }, transaction: transaction,
            cancellationToken: cancellationToken));

        foreach (var genre in movie.Genres)
        {
            await connection.ExecuteAsync(new CommandDefinition("""
                                                                insert into genres (movieid, name)
                                                                values (@MovieId, @Name)
                                                                """, new { MovieId = movie.Id, Name = genre },
                transaction: transaction, cancellationToken: cancellationToken));
        }

        var result = await connection.ExecuteAsync(new CommandDefinition($"""
                                                                          update movies
                                                                          set title = @Title,
                                                                          slug = @Slug,
                                                                          yearofrelease = @YearOfRelease
                                                                          where id = @Id
                                                                          """, movie, transaction: transaction,
            cancellationToken: cancellationToken));

        transaction.Commit();
        connection.Close();
        return result > 0;
    }

    public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        using var connection = await connectionFactory.CreateConnectionAsync(cancellationToken);

        using var transaction = connection.BeginTransaction();

        await connection.ExecuteAsync(new CommandDefinition("""
                                                            delete from genres where movieid = @id
                                                            """, new { id = id }));
        var result = await connection.ExecuteAsync(new CommandDefinition("""
                                                                         delete from movies where id = @id
                                                                         """, new { id }, transaction: transaction,
            cancellationToken: cancellationToken));

        transaction.Commit();
        connection.Close();
        return result > 0;
    }

    public async Task<bool> ExistsByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        using var connection = await connectionFactory.CreateConnectionAsync(cancellationToken);

        var exist = await connection.ExecuteScalarAsync<bool>(
            new CommandDefinition("select count(1) from movies where id = @id", new { id },
                cancellationToken: cancellationToken));

        connection.Close();
        return exist;
    }
}