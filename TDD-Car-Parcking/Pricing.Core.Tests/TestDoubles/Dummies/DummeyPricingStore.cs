using Pricing.Core.Domain;
using Pricing.Core.Tests.Domain;

namespace Pricing.Core.Tests;

public class DummeyPricingStore : IPricingStore
{
    public Task<bool> SaveAsync(PricingTable pricingRequest, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}