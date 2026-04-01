using Dapper;
using FluentAssertions;
using Newtonsoft.Json;
using Pricing.Core.Domain;
using Pricing.Core.Tests.Domain;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace Pricing.Infrustructure.Tests;

public class PricingStoreSpecification: IClassFixture<PostgresSqlFixture>
{
    
    private readonly IDbConnectionFactory _dbConnectionFactory;
    public PricingStoreSpecification(PostgresSqlFixture fixture)
    {
        _dbConnectionFactory = fixture._connectionFactory;
    }

    [Fact]
    public void Sould_throw_exception_if_missing_connection()
    {
        var create = () => new PostgresPricingStore(null);
        
        create.Should().Throw<ArgumentNullException>();
    }



    [Fact]
    public async void Should_return_true_if_save_is_succeeded()
    {
        IPricingStore store = new PostgresPricingStore(_dbConnectionFactory);
        var pricingTable = CreatePricingTable();

        var result = await store.SaveAsync(pricingTable,default);
        
        result.Should().BeTrue();
    }

    private static PricingTable CreatePricingTable()
    {
        var pricingTable = new PricingTable(new[] { new PriceTier(0.5, 24) });
        return pricingTable;
    }

    [Fact]
    public async void Should_insert_if_not_exist()
    {
        IPricingStore store = new PostgresPricingStore(_dbConnectionFactory);
        
        await CleanUpPricingStore();

        var result = await InsertPricingTable(store);

        result.Should().BeTrue();
        
    }

    private async Task CleanUpPricingStore()
    {
        var connection = await _dbConnectionFactory.CreateConnectionAsync();
        await connection.ExecuteAsync("truncate table Pricing;");
    }
    private static async Task<bool> InsertPricingTable(IPricingStore store)
    {
        var pricingTable = CreatePricingTable();
        var result = await store.SaveAsync(pricingTable,default);
        return result;
    }

    
    [Fact]
    public async Task Should_replace_if_already_exists()
    {
        
        var pricingTable = new PricingTable(new[] { new PriceTier(1, 24) });
        IPricingStore store = new PostgresPricingStore(_dbConnectionFactory);
        await store.SaveAsync(pricingTable, default);
        
        var newPricingTable = new PricingTable(new[] { new PriceTier(4, 24) });
        var restult = await store.SaveAsync(newPricingTable, default);

        restult.Should().BeTrue();
        
        var connection = await _dbConnectionFactory.CreateConnectionAsync();
        var data = await connection.QueryAsync(
            @"SELECT * FROM pricing"
            );

        
        data.Should().HaveCount(1)
            .And
            .Subject.First().document.Equals(SerialisePricingTable(newPricingTable));
        
    }

    private static string SerialisePricingTable(PricingTable newPricingTable)
    {
        var serializedPricingTable = JsonSerializer.Serialize(newPricingTable);
        return serializedPricingTable;
    }
}