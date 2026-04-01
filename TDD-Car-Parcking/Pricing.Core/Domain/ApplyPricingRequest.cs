using Pricing.Core.Tests.Domain;

namespace Pricing.Core.Tests.Domain;

public record ApplyPricingRequest(IReadOnlyCollection<PriceTierRequest> Tiers);

public record PriceTierRequest(int HourLimit,double Price);

