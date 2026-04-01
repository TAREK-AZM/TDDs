using Microsoft.AspNetCore.Mvc;
using Pricing.Api.Tests;
using Pricing.Core.Tests;
using Pricing.Core.Tests.Domain;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddScoped<IPricingManager, PricingManager>();

var app = builder.Build();



app.MapGet("/", () => "Hello World!");

// here IPricingManager is represent the test's IPricing 
// example : Should_return_200_when_pricing_manager_returns_true
// var api = await CreateApiWithPricingManager<StubApplySucceedPricingManager>();
// the create method will remove all the other IPricingManger 
// and inject it's current test need's :StubApplySucceedPricingManager

app.MapPut("/PricingTable", async (
        IPricingManager pricingManager,
        ApplyPricingRequest request,
        CancellationToken cancellationToken
        ) =>
    {
        try
        {
            
            var result = await pricingManager.HandelRequestAsync(request, cancellationToken);

            return result ?Results.Ok() : Results.BadRequest();
            
        }
        catch (PricingTiersInvalidException ex)
        {
            return Results.Problem();

        }
    }
);

app.Run();