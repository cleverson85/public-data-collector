using PublicDataCollector.Application.Gateway;
using PublicDataCollector.Domain.Entities;
using PublicDataCollector.Domain.Repositories;

namespace PublicDataCollector.Jobs.Jobs;

public sealed class CurrencyRateUpdaterJob
{
    private readonly ICurrencyRateRepository _currencyRateRepository;
    private readonly IExchangeRateApiClient _exchangeRateApiClient;

    public CurrencyRateUpdaterJob(ICurrencyRateRepository currencyRateRepository, IExchangeRateApiClient exchangeRateApiClient)
    {
        _currencyRateRepository = currencyRateRepository;
        _exchangeRateApiClient = exchangeRateApiClient;
    }

    public async Task ExecuteAsync()
    {
        List<CurrencyRate> currencyRates = await _exchangeRateApiClient.GetExchangeRateAsync("USD", new CancellationToken());

        await _currencyRateRepository.UpsertRatesAsync(currencyRates);
    }
}
