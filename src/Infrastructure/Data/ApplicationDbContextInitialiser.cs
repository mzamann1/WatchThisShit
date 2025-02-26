using System.Data;
using Dapper;
using Microsoft.Extensions.Logging;
using WatchThisShit.Application.Common.Interfaces;

namespace WatchThisShit.Infrastructure.Data;

public class ApplicationDbContextInitialiser
{
    private readonly IDbConnectionFactory _connectionFactory;
    private readonly ILogger<ApplicationDbContextInitialiser> _logger;

    public ApplicationDbContextInitialiser(ILogger<ApplicationDbContextInitialiser> logger,
        IDbConnectionFactory connectionFactory)
    {
        _logger = logger;
        _connectionFactory = connectionFactory;
    }

    public async Task InitialiseAsync()
    {
        using IDbConnection? connection = await _connectionFactory.CreateConnectionAsync();
        try
        {
            await connection.ExecuteAsync(
                """
                CREATE table IF NOT EXISTS movies (
                id UUID NOT NULL PRIMARY KEY,
                title VARCHAR(50) NOT NULL,
                slug VARCHAR(50) NOT NULL,
                yearOfRelease integer NOT NULL)
                """);

            await connection.ExecuteAsync(
                "create unique index if not exists movies_slug_idx on movies using btree(slug)");
            await connection.ExecuteAsync("""
                                          create table if not exists genres (
                                              movieId UUID references movies(id),
                                              name VARCHAR(50) NOT NULL
                                          )
                                          """);

            connection.Close();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while initialising the database.");
            connection.Close();
            throw;
        }
    }
}
