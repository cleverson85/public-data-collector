using PublicDataCollector.Domain.Entities;

namespace PublicDataCollector.Domain.Repositories;

public interface ICurrencyRateRepository
{
    Task UpsertRatesAsync(IEnumerable<CurrencyRate> rates);
    Task<List<CurrencyRate>> GetAllAsync();
}
