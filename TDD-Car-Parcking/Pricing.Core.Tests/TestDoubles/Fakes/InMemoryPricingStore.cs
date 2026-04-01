using Pricing.Core.Domain;
using Pricing.Core.Tests.Domain;

namespace Pricing.Core.Tests.Fakes;

public class InMemoryPricingStore : IPricingStore // fake
{
    private PricingTable _savedPricingTable; // simulating this stored in memory may be dictionary ect ..
    public Task<bool> SaveAsync(PricingTable pricingTable, CancellationToken cancellationToken)
    {
        _savedPricingTable = pricingTable;
        return Task.FromResult(true);
    }

    public PricingTable GetSavedPricingTable()
    {
        return _savedPricingTable;
    }

    public  void Clean()
    {
        _savedPricingTable = null;
    }
}