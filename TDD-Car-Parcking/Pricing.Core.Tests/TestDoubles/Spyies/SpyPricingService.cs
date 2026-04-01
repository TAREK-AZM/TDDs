using Pricing.Core.Domain;
using Pricing.Core.Tests.Domain;

namespace Pricing.Core.Tests.Spyies;

public class SpyPricingService : IPricingStore
{
    public int NumberOfSaves { get; private set; }

    public Task<bool> SaveAsync(PricingTable pricingRequest, CancellationToken cancellationToken)
    {
        NumberOfSaves++;
        return Task.FromResult(true);
    }

}