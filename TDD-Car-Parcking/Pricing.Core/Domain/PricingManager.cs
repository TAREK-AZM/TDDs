using Pricing.Core.Domain;
using Pricing.Core.Domain.Extensions;
using Pricing.Core.Tests.Domain;

namespace Pricing.Core.Tests;

public class PricingManager: IPricingManager // like service layer
{
    IPricingStore _pricingStore; // like repository layer
    public PricingManager(IPricingStore pricingStore)
    {
        _pricingStore = pricingStore;
    }
    public  Task<bool> HandelRequestAsync(ApplyPricingRequest request, CancellationToken token)
    {
        ArgumentNullException.ThrowIfNull(request);
        var pricingTable = request.MappToPricingTable();

        return  _pricingStore.SaveAsync(pricingTable, token);
    }
}


public interface IPricingManager
{
    Task<bool> HandelRequestAsync(ApplyPricingRequest request, CancellationToken token);

}

