using Renci.SshNet.Security;
using Testcontainers.PostgreSql;

namespace Pricing.Infrustructure.Tests;


// note here we not worked by constructor setup and dispose teardown because they are synchronous 
// but now we are dealing with asynchronous operation like I/O , DB ,....
// so mixing the both => leads to problems

// package to postgresContainer : TestContainers.Postgres
/*
     PostgreSQL container starts
        Test1
        Test2
        Test3
    PostgreSQL container stops
    
    ============ Not ==========
    start DB
    Test1
    stop DB

    start DB
    Test2
    stop DB
    
    Note !!! : Because starting a database per test would be very slow.
*/
public class PostgresSqlFixture: IAsyncLifetime
{
    private readonly PostgreSqlContainer _postgreSqlContainer 
        = new PostgreSqlBuilder().Build();

    public IDbConnectionFactory _connectionFactory;
    
    
    public async Task InitializeAsync()
    {
        await _postgreSqlContainer.StartAsync();
        // this will the get the connection string that will be used in other tests
        _connectionFactory = new NpgsqlConnectionFactory(_postgreSqlContainer.GetConnectionString());

        await new DataInitialiser(_connectionFactory).InitialiseAsync();
    }

    public  async Task DisposeAsync()
    {
        await _postgreSqlContainer.DisposeAsync();
    }
}