using System.Data;
using Npgsql;

namespace Pricing.Infrustructure.Tests;

public class NpgsqlConnectionFactory : IDbConnectionFactory
{
    private readonly string _connectionString;
    public NpgsqlConnectionFactory(string getConnectionString)
    {
        _connectionString = getConnectionString;
    }

    public async Task<IDbConnection> CreateConnectionAsync()
    {
        var connection = new NpgsqlConnection(_connectionString);
        await connection.OpenAsync();
        
        return connection;

    }
}