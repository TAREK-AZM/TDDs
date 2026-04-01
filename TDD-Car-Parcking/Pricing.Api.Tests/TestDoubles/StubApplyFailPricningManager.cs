using Pricing.Core.Tests;
using Pricing.Core.Tests.Domain;

namespace Pricing.Api.Tests;

public class StubApplyFailPricningManager:IPricingManager
{
    public Task<bool> HandelRequestAsync(ApplyPricingRequest request, CancellationToken token)
    {
        return Task.FromResult(false);
    }
}