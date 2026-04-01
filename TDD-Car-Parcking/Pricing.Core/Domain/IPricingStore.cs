using Pricing.Core.Tests;
using Pricing.Core.Tests.Domain;

namespace Pricing.Core.Domain;

public interface IPricingStore
{
    Task<bool> SaveAsync(PricingTable pricingTable, CancellationToken cancellationToken);
}