using System.Collections.Immutable;

namespace Pricing.Core.Tests.Domain;

public class PricingTable
{
    public IEnumerable<PriceTier> _PriceTiers = new HashSet<PriceTier>();
    public int MaxDialyPrice { get; set; }
    public PricingTable(){}
    public PricingTable(IEnumerable<PriceTier> table)
    {
        
        _PriceTiers = table?.OrderBy(tier => tier.HourLimit).ToImmutableArray() ?? throw new ArgumentNullException();

        if (!_PriceTiers.Any())
        {
            throw new ArgumentException("Pricing table can not be empty", nameof(_PriceTiers));
        }
        
    }

    
    
    public double GetMaxDailyPrice()
    {
        var total = 0.0;
        total = SumTotalPrice();
        
        if (MaxDialyPrice == 0)
            return total;
        
        return Math.Min(total, MaxDialyPrice);
    }

    public bool IsMaxDailyPriceValide()
    {
        var total = GetMaxDailyPrice();
        if(MaxDialyPrice > total)
            throw new ArgumentException();
        return true;
    }
    
    private double SumTotalPrice()
    {
        var total = 0.0;
        var includedHours = 0;

        foreach (var tier in _PriceTiers)
        {
            total += tier.Price *(tier.HourLimit - includedHours);
            includedHours = tier.HourLimit;
        }

        return total;
    }


    public bool IsCover24HoursLimit()
    {
        if (_PriceTiers.Last().HourLimit < 24)
            throw new ArgumentException();
        return true;
    }
    
}