using Microsoft.AspNetCore.Mvc;
using MinimalEndpoints.Abstractions;
using PublicDataCollector.Application.CurrencyRates;
using WebApp.Endpoints;

namespace PublicDataCollector.WebApi.Endpoints.V1;

public class Rates : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("rates", async (IGetCurrencyRatesService operation, CancellationToken cancellationToken) =>
        {
            var result = await operation.GetAllRatesAsync(cancellationToken);
            return result;
        })
        .WithTags(EndpointSchema.Rates)
        .MapToApiVersion(1);

        app.MapGet("rates/filter", async (IGetCurrencyRatesService operation,
            [FromQuery] string? code,
            [FromQuery] DateTime? date,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10,
            CancellationToken cancellationToken = default) =>
        {
            var result = await operation.GetRatesByFilterAsync(code, date, page, pageSize, cancellationToken);
            return result;
        })
        .WithTags(EndpointSchema.Rates)
        .MapToApiVersion(1);
    }
}