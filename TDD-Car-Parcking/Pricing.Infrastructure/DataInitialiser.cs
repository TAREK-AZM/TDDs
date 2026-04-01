using Dapper;
using Pricing.Infrustructure.Tests;

namespace Pricing.Infrustructure;

public class DataInitialiser 
{
    private readonly IDbConnectionFactory _connectionFactory;
    public DataInitialiser(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }
    
    public async Task InitialiseAsync()
    {

        var connection = await _connectionFactory.CreateConnectionAsync();
        await connection.ExecuteAsync(
            @"CREATE TABLE IF NOT EXISTS pricing (
        Key TEXT PRIMARY KEY ,
        document TEXT NOT NULL
    );"
        );

    }
    
    
}