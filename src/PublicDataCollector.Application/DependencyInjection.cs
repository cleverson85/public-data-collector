using Microsoft.Extensions.DependencyInjection;
using PublicDataCollector.Application.CurrencyRates;

namespace PublicDataCollector.WebApi;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IGetCurrencyRatesService, GetCurrencyRatesService>();

        return services;
    }
}
