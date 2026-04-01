using System.Text.Json;
using Dapper;
using Pricing.Core.Domain;
using Pricing.Core.Tests.Domain;


namespace Pricing.Infrustructure.Tests;

public class PostgresPricingStore: IPricingStore
{
    private readonly IDbConnectionFactory _dbConnectionFactory;
    public PostgresPricingStore(IDbConnectionFactory dbConnectionFactory)
    {
        ArgumentNullException.ThrowIfNull(dbConnectionFactory);
        _dbConnectionFactory = dbConnectionFactory;
    }

    public async Task<bool> SaveAsync(PricingTable pricingTable, CancellationToken cancellationToken)
    {
        var data = new DbRecord(JsonSerializer.Serialize(pricingTable));
        var connection = await _dbConnectionFactory.CreateConnectionAsync();
        
        var ExitInt= await connection.ExecuteAsync(
            @"INSERT INTO Pricing(key,document) VALUES (@key,@document)
            ON CONFLICT (KEY) DO UPDATE
            SET document = excluded.document;
            ",
            data
            );
        
        return ExitInt > 0;
    }
    
    private record DbRecord(string document,string key="Active");
}