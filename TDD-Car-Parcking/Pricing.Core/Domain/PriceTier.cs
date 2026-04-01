using Pricing.Core.Tests.Domain.Exceptions;

namespace Pricing.Core.Tests.Domain;

public class PriceTier
{
    public double Price { get; set; }
    public int HourLimit { get; set; }

    public PriceTier(){}
    public PriceTier(double price, int hourLimit)
    {
        
        if (price <= 0)
            throw new InvalidPriceTierException("Invalid price tier: price cannot be negative");
        if(hourLimit is <= 0 or > 24)
            throw new InvalidPriceTierException("Invalid price tier: hour limit cannot be less than 24 and less than or equal to 0");
        
        Price = price;
        HourLimit = hourLimit;
    }
}