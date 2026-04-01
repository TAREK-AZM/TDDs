using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Pricing.Api;
using Pricing.Infrustructure;
using Pricing.Infrustructure.Tests;
using Testcontainers.PostgreSql;

namespace Pricing.Acceptance.Tests;

public class ApiFactory:WebApplicationFactory<IApiAssemblyMarker>
,IAsyncLifetime{

    private readonly PostgreSqlContainer _postgreSqlContainer 
        = new PostgreSqlBuilder().Build();

    public IDbConnectionFactory _connectionFactory;
    
    
    public async Task InitializeAsync()
    {
        await _postgreSqlContainer.StartAsync();
        
        _connectionFactory = new NpgsqlConnectionFactory(_postgreSqlContainer.GetConnectionString());

        await new DataInitialiser(_connectionFactory)
            .InitialiseAsync();
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {

        builder.ConfigureTestServices(services =>
        {
            services.RemoveAll(typeof(IDbConnectionFactory));
            services.AddSingleton<IDbConnectionFactory>(_=> 
                new NpgsqlConnectionFactory(_postgreSqlContainer.GetConnectionString())
                );

        });
    }
    
    
    
    public  async Task DisposeAsync()
    {
        await _postgreSqlContainer.DisposeAsync();
    }
    
    
    
}