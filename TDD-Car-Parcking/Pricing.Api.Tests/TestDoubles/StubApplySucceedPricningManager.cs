using Pricing.Core.Tests;
using Pricing.Core.Tests.Domain;

namespace Pricing.Api.Tests;

public class StubApplySucceedPricningManager: IPricingManager
{
    public Task<bool> HandelRequestAsync(ApplyPricingRequest request, CancellationToken token)
    {
        return Task.FromResult(true);
    }
    
    
}