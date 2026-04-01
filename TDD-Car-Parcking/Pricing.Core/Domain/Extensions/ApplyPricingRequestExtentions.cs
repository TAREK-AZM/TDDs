using Pricing.Core.Tests;
using Pricing.Core.Tests.Domain;

namespace Pricing.Core.Domain.Extensions;

public static class ApplyPricingRequestExtentions
{

    public static PricingTable MappToPricingTable(this ApplyPricingRequest applyPricingRequest)
    {
        PricingTable pricingTable = new PricingTable();
        pricingTable._PriceTiers = applyPricingRequest.Tiers.Select(tier => new PriceTier(tier.Price,tier.HourLimit));
        return pricingTable;
    }
}