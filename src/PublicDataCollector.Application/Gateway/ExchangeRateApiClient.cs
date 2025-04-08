using PublicDataCollector.Domain.Entities;
using System.Text.Json;

namespace PublicDataCollector.Application.Gateway;

public class ExchangeRateApiClient : IExchangeRateApiClient
{
    private readonly IHttpClientFactory _factory;

    public ExchangeRateApiClient(IHttpClientFactory factory)
    {
        _factory = factory;
    }

    public async Task<List<CurrencyRate>> GetExchangeRateAsync(string rateCode, CancellationToken cancellationToken)
    {
        try
        {
            using var httpClient = _factory.CreateClient("clientProvider");
            var response = await httpClient.GetAsync(rateCode, cancellationToken);

            response.EnsureSuccessStatusCode();

            var contentStream = await response.Content.ReadAsStreamAsync(cancellationToken);
            var jsonDoc = await JsonDocument.ParseAsync(contentStream, cancellationToken: cancellationToken);

            var rates = jsonDoc.RootElement.GetProperty("conversion_rates");

            var currencyRates = new List<CurrencyRate>();
            foreach (var rate in rates.EnumerateObject())
            {
                currencyRates.Add(new CurrencyRate
                {
                    TargetCurrency = rateCode,
                    Code = rate.Name,
                    Rate = rate.Value.GetDecimal()
                });
            }

            return currencyRates;
        }
        catch
        {
            return [];
        }
    }
}

public interface IExchangeRateApiClient
{
    Task<List<CurrencyRate>> GetExchangeRateAsync(string rateCode, CancellationToken cancellationToken);
}
