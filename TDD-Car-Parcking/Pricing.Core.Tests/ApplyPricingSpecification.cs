using FluentAssertions;
using NSubstitute;
using Pricing.Core.Domain;
using Pricing.Core.Tests.Domain;
using Pricing.Core.Tests.Fakes;
using Pricing.Core.Tests.Mocks;
using Pricing.Core.Tests.Spyies;
using Pricing.Core.Tests.Stubs;

namespace Pricing.Core.Tests;

public class ApplyPricingSpecification : IClassFixture<FakeFixture>,IDisposable
{

    private readonly PricingManager pricingManager;
    private readonly InMemoryPricingStore memoryPricingStore;
    
    [Fact]
    public async void Should_throw_argument_null_exception_if_request_is_null()
    {
        var pricingManager = new PricingManager(new DummeyPricingStore());

        var handelRequest = ()=>pricingManager.HandelRequestAsync(null, default);
        
       await handelRequest.Should().ThrowAsync<ArgumentNullException>();
    }
    
    [Fact]
    public async void Should_return_true_if_request_is_succeeded()
    {
        var pricingManager = new PricingManager(new StubSuccessPricingStore());
        var result =  await pricingManager.HandelRequestAsync(CreateRequest(), default);
        
        result.Should().BeTrue();
    }
    
    [Fact]
    public async void Should_return_false_if_fails_to_save()
    {
        var pricingManager = new PricingManager(new StubFailPricingStore());
        var result =  await pricingManager.HandelRequestAsync(CreateRequest(), default);
        
        result.Should().BeFalse();
    }


    [Fact]
    public void Should_save_only_once()
    {
        var spyPricingService = new SpyPricingService();
        var pricingManager = new PricingManager(spyPricingService);
        _ =   pricingManager.HandelRequestAsync(CreateRequest(), default);

        spyPricingService.NumberOfSaves.Should().Be(1);

    }

    [Fact]
    public async void Should_save_expected_data()
    {
        var expectedPricingTable = new PricingTable(new []{ new PriceTier(2,1),new PriceTier(3,2)});
        var mockPricingStore = new MockPricingStore();
        mockPricingStore.ExpectedToSave(expectedPricingTable); // setter
        
        var pricingManager = new PricingManager(mockPricingStore);
        _= await pricingManager.HandelRequestAsync(CreateRequest(), default);
        
        mockPricingStore.Verify(); // self Verify
        
    } 
    
    [Fact]
    public async void Should_save_expected_data_nSubstitute()
    {
        var expectedPricingTable = new PricingTable(new []{ new PriceTier(2,1),new PriceTier(3,2)});
        var mockPricingStore = Substitute.For<IPricingStore>();
        
        var pricingManager = new PricingManager(mockPricingStore);
        _= await pricingManager.HandelRequestAsync(CreateRequest(), default);

        mockPricingStore.Received().SaveAsync(Arg.Is<PricingTable>(table => 
                MatchTiersCount(table, expectedPricingTable)
                &&
                MatchTiers(table, expectedPricingTable)
          )  
          ,default);
    }

    private static bool MatchTiersCount(PricingTable table, PricingTable expectedPricingTable)
    {
        return table._PriceTiers.Count() == expectedPricingTable._PriceTiers.Count();
    }

    private static bool MatchTiers(PricingTable table, PricingTable expectedPricingTable)
    {
        return table._PriceTiers.All(tier=> 
            expectedPricingTable._PriceTiers.Any(
                e=>e.Price.Equals(tier.Price) && 
                   e.HourLimit.Equals(tier.HourLimit)));
    }


    [Fact]
    public void Should_save_equivalent_data_to_storage() // fake
    {
        var expectedPricingTable = new PricingTable(new []{ new PriceTier(2,1),new PriceTier(3,2)});
        var memoryPricingStore = new InMemoryPricingStore();
        var pricingManager = new PricingManager(memoryPricingStore);
        
        _= pricingManager.HandelRequestAsync(CreateRequest(), default);
        
        memoryPricingStore.GetSavedPricingTable().Should().BeEquivalentTo(expectedPricingTable);
        
    }


    public ApplyPricingSpecification()
    {
        memoryPricingStore = new InMemoryPricingStore();
        pricingManager = new PricingManager(memoryPricingStore);
    }
    
    
    [Fact]
    public void LifeCycle_of_Test() // fake
    {
        var expectedPricingTable = new PricingTable(new []{ new PriceTier(2,1),new PriceTier(3,2)});
     
        
        _= pricingManager.HandelRequestAsync(CreateRequest(), default);
        
        memoryPricingStore.GetSavedPricingTable().Should().BeEquivalentTo(expectedPricingTable);
        
    }
    
    
    
    private static ApplyPricingRequest CreateRequest()
    {
        return new ApplyPricingRequest(
            new []
            {
                new PriceTierRequest(1,2),
                new PriceTierRequest(2,3)
            }
            );
    }

    public void Dispose()
    {
        memoryPricingStore.Clean();
    }
}

public class FakeFixture
{
    public FakeFixture()
    {
        
    }
    
    public void Dispose()
    {
    }
}