using FluentAssertions;
using Pricing.Core.Domain;
using Pricing.Core.Tests.Domain;

namespace Pricing.Core.Tests.Mocks;

public class MockPricingStore : IPricingStore
{
    private PricingTable _expectedPricingTable;
    private PricingTable _savedPricingTable;
    public Task<bool> SaveAsync(PricingTable request, CancellationToken cancellationToken)
    {
        _savedPricingTable = request;
        return Task.FromResult(true);
    }

    public void Verify()
    {
        _savedPricingTable.Should().BeEquivalentTo(_expectedPricingTable);
    }

    public void ExpectedToSave(PricingTable expectedPricingTableToSve)
    {
        _expectedPricingTable = expectedPricingTableToSve;
    }
    
}