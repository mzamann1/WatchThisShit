using Dapper;
using WatchThisShit.Application.Models;

namespace WatchThisShit.Application.Database;

public class DbInitializer
{
    private readonly IDbConnectionFactory _connectionFactory;

    public DbInitializer(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task InitializeAsync()
    {
        using var connection = await _connectionFactory.CreateConnectionAsync();

        await connection.ExecuteAsync(
            $"""
             CREATE table IF NOT EXISTS movies (
             id UUID NOT NULL PRIMARY KEY,
             title VARCHAR(50) NOT NULL,
             slug VARCHAR(50) NOT NULL,
             yearOfRelease integer NOT NULL)
             """);

        await connection.ExecuteAsync($"create unique index if not exists movies_slug_idx on movies using btree(slug)");
        await connection.ExecuteAsync("""
                                      create table if not exists genres (
                                          movieId UUID references movies(id),
                                          name VARCHAR(50) NOT NULL
                                      )
                                      """);

    }
}