using System.Data;

namespace Pricing.Infrustructure.Tests;

public interface IDbConnectionFactory
{
    
    Task<IDbConnection> CreateConnectionAsync();
}