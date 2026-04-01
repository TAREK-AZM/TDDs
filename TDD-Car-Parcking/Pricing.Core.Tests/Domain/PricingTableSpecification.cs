using FluentAssertions;
using Pricing.Core.Tests.Domain;

namespace Pricing.Core.Tests;

public class PricingTableSpecification
{
    [Fact]
    public void Should_throw_argument_null_exception_if_price_table_is_null()
    {

        var createPricingTable = ()=>new PricingTable(null);
        createPricingTable.Should().Throw<ArgumentNullException>();
    }


    [Fact]
    public void Should_throw_argument_exception_if_price_table_is_empty()
    {
        var createPricingTable = () => new PricingTable(Array.Empty<PriceTier>());
        createPricingTable.Should().Throw<ArgumentException>()
            .WithParameterName(nameof(PricingTable))
            .WithMessage("Pricing table can not be empty*");
    }

    [Fact] 
    public void Sould_contains_at_least_one_priceTier()
    {
        // range
        var pricingTable = new PricingTable( new []
            {
                CreateTier()
            }
            );
        
        pricingTable._PriceTiers.Should().HaveCount(1);
        
    }


    [Fact]
    public void Price_tiers_should_be_ordered_by_hour_limit()
    {
        // range
        var pricingTable = new PricingTable( new []
            {
                CreateTier(2),
                CreateTier(1),
            }
            );
        
        
        // action
        var orderedTiers = pricingTable._PriceTiers.OrderBy(tier => tier.HourLimit);
        
        // assert
        orderedTiers.Should().BeInAscendingOrder(tier => tier.HourLimit);
        
    }

    [Fact]
    public void Maximum_daily_price_should_be_calculated_from_PriceTiers_if_not_empty()
    {
        // range
        var pricingTable = new PricingTable( new []
        {
            CreateTier(2,5),
            CreateTier(10,4),
            CreateTier(24,3)
        });
        
        pricingTable.GetMaxDailyPrice().Should().Be(84);
    }
    
    
    
    
    [Fact]
    public void Maximum_daily_price()
    {
        // range
        var pricingTable = new PricingTable(new []
        {
            CreateTier(2,5),
            CreateTier(10,4),
            CreateTier(24,3)
        });
        pricingTable.MaxDialyPrice = 20;

        
        pricingTable.GetMaxDailyPrice().Should().Be(20);

    }


    [Fact]
    public void Should_fail_if_tiers_not_cover_24_hour_limit()
    {
        // range
        var pricingTable = new PricingTable(new []
        {
            CreateTier(2,5),
            CreateTier(10,4),
            CreateTier(23,3)
        });
       
        Action action = () => pricingTable.IsCover24HoursLimit();
        action.Should().Throw<ArgumentException>();
        
    }

    [Fact]
    public void Should_fail_if_Maximum_daily_price_is_greater_than_24_hour_limit()
    {
        // range
        var pricingTable = new PricingTable( new []
        {
            CreateTier(2,5),
            CreateTier(10,4),
            CreateTier(24,3)
        });
        pricingTable.MaxDialyPrice = 85;
        
        
        // action
        Func<bool> action = () => pricingTable.IsMaxDailyPriceValide();
        
        // assert
        action.Should().Throw<ArgumentException>();
    }
    
    

    
    
    public static PriceTier CreateTier(int hourLimit=24,double price=0) 
        => new PriceTier { HourLimit = hourLimit , Price = price };
    
    
}



