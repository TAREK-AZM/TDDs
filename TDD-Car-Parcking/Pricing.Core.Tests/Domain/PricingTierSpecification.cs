using FluentAssertions;
using Pricing.Core.Tests.Domain;
using Pricing.Core.Tests.Domain.Exceptions;

namespace Pricing.Core.Tests;

public class PricingTierSpecification
{
    
    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(26)]
    public void Should_throw_invalid_price_tier_exception_when_hour_limit_is_not_valid(int hour)
    {
        
        var create = () => new PriceTier(25,hour);
        create.Should().ThrowExactly<InvalidPriceTierException>()
            .WithMessage("Invalid Price Tier:*");
    }
    
    
    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(-5)]
    public void Should_throw_invalid_price_tier_exception_when_price_not_valid(int price)
    {
        
        var create = () => new PriceTier(price,2);
        create.Should().ThrowExactly<InvalidPriceTierException>()
            .WithMessage("Invalid Price Tier:*");
    }
    
    
}

