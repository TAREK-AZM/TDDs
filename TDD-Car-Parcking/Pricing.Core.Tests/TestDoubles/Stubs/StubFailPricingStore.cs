using Pricing.Core.Domain;
using Pricing.Core.Tests.Domain;

namespace Pricing.Core.Tests.Stubs;

public class StubFailPricingStore : IPricingStore
{
    public Task<bool> SaveAsync(PricingTable pricingRequest, CancellationToken cancellationToken)
    {
        return Task.FromResult(false);
    }
}