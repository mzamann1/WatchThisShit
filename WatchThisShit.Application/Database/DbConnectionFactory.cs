using System.Data;
using Npgsql;

namespace WatchThisShit.Application.Database;

public interface IDbConnectionFactory
{
    Task<IDbConnection> CreateConnectionAsync();
}

public class NpgSQLConnectionFactory : IDbConnectionFactory
{
    private readonly string _connectionString;

    public NpgSQLConnectionFactory(string connectionString)
    {
        _connectionString = connectionString;
    }


    public async Task<IDbConnection> CreateConnectionAsync()
    {
        var connection = new NpgsqlConnection(_connectionString);
        await connection.OpenAsync();
        return connection;
    }
}