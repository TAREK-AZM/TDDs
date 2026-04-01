using Pricing.Core.Tests;
using Pricing.Core.Tests.Domain;

namespace Pricing.Api.Tests;

public class StubExceptionPricingManager:IPricingManager
{
    public Task<bool> HandelRequestAsync(ApplyPricingRequest request, CancellationToken token)
    {
        throw new PricingTiersInvalidException();
    }
}