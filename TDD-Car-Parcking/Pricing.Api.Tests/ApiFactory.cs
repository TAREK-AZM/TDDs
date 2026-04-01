using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Pricing.Core.Tests;
using Pricing.Infrustructure;
using Pricing.Infrustructure.Tests;

namespace Pricing.Api.Tests;


// Note !!! : the IApiAssembleMarker is just a marker that links 
// between the two project so i make changes dont need to modify this class
public class ApiFactory: WebApplicationFactory<IApiAssemblyMarker>
{
    private readonly Action<IServiceCollection> _configure;

    public ApiFactory(Action<IServiceCollection> configure)
    {
        _configure = configure;
    }
    
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Test");
        
        builder.ConfigureTestServices(
            services =>
            {
                services.RemoveAll(typeof(IDbConnectionFactory));
                services.RemoveAll(typeof(DataInitialiser));
                //services.RemoveAll(typeof(IPricingManager));
                
                _configure(services);
            }
            
            );
    }
}