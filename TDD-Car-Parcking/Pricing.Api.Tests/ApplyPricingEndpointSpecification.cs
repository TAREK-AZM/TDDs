using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Pricing.Core.Domain;
using Pricing.Core.Domain.Extensions;
using Pricing.Core.Tests;
using Pricing.Core.Tests.Domain;
using Pricing.Core.Tests.Fakes;

namespace Pricing.Api.Tests;

public class ApplyPricingEndpointSpecification
{
    private readonly string requestUri = "/PricingTable";
    
    [Fact]
    public async Task Should_return_500_error_if_happened_exception()
    { 
        
       await using var api =  CreateApiWithPricingManager<StubExceptionPricingManager>();
       using var client = api.CreateClient();
       
       var response =  await client.PutAsJsonAsync(requestUri,
           createPricingTableRequest()
           );
       
           response.StatusCode.Should().Be(HttpStatusCode.InternalServerError);
    }


    [Fact]
    public async Task Should_return_400_error_when_pricing_manager_returns_false()
    {
        // Note !!! : when we use ApiFactory we configure the application as it is in the program.cs
        var api = CreateApiWithPricingManager<StubApplyFailPricningManager>();
        using var client = api.CreateClient();
        
        var response =  await client.PutAsJsonAsync(requestUri,
            createPricingTableRequest()
        );
        
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest); 
    }

    [Fact]
    public async Task Should_send_request_to_pricing_manager()
    {
        var memoryPricingStore = new InMemoryPricingStore();
        using var api = new ApiFactory(services =>
        {
            services.RemoveAll(typeof(IPricingStore));
            // use real concrete IPricingStore
            services.TryAddScoped<IPricingStore>(_=> memoryPricingStore);
        });

        var client = api.CreateClient();
       await  client.PutAsJsonAsync(requestUri,createPricingTableRequest());
        
        memoryPricingStore.GetSavedPricingTable()
            .Should().BeEquivalentTo(createPricingTableRequest().MappToPricingTable());

    }
    
    
    
    [Fact]
    public async Task Should_return_200_when_pricing_manager_returns_true()
    {
        // Note !!! : when we use ApiFactory we configure the application as
        // it is in the program.cs
        var api = CreateApiWithPricingManager<StubApplySucceedPricningManager>();
        using var client = api.CreateClient();
        
        var response =  await client.PutAsJsonAsync(requestUri,
            createPricingTableRequest()
        );
        
        response.StatusCode.Should().Be(HttpStatusCode.OK); 
    }

    private static ApplyPricingRequest createPricingTableRequest()
    {
        return new ApplyPricingRequest(new []
        {
            new PriceTierRequest(24,0.5)
        });
    }


    // this is used to override and new inject the dependencies in the TestServer
    // like the: StubApplyFailPricningManager, PricingTiersInvalidException
    private static ApiFactory CreateApiWithPricingManager<T>()
    where T: class, IPricingManager
    
    {
          var api = new ApiFactory(services =>
        {
            services.RemoveAll(typeof(IPricingManager));
            services.TryAddScoped<IPricingManager, T>();
        });

        return api;
    }
    
    
    
}