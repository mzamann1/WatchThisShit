using System.Data;
using Npgsql;
using WatchThisShit.Application.Common.Interfaces;

namespace WatchThisShit.Infrastructure.Data;

public class NpgSQLConnectionFactory : IDbConnectionFactory
{
    private readonly string _connectionString;

    public NpgSQLConnectionFactory(string connectionString)
    {
        _connectionString = connectionString;
    }


    public async Task<IDbConnection> CreateConnectionAsync(CancellationToken cancellationToken = default)
    {
        NpgsqlConnection connection = new(_connectionString);
        await connection.OpenAsync(cancellationToken);
        return connection;
    }
}
