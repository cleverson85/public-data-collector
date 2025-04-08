using Microsoft.EntityFrameworkCore;
using PublicDataCollector.Application.Data;
using PublicDataCollector.Domain.Entities;
using PublicDataCollector.Domain.Repositories;

namespace PublicDataCollector.Infrastructure.Repositories;

public sealed class CurrencyRateRepository : ICurrencyRateRepository
{
    private readonly ApplicationDbContext _applicationDbContext;

    public CurrencyRateRepository(ApplicationDbContext applicationDbContext)
    {
        _applicationDbContext = applicationDbContext;
    }

    public async Task<List<CurrencyRate>> GetAllAsync() => await _applicationDbContext.CurrencyRates.ToListAsync();

    public async Task UpsertRatesAsync(IEnumerable<CurrencyRate> rates)
    {
        foreach (var rate in rates)
        {
            var existing = await _applicationDbContext.CurrencyRates
               .FirstOrDefaultAsync(c => c.Code == rate.Code
                                    && c.Rate == rate.Rate
                                    && c.Date == rate.Date);

            if (existing is null)
                _applicationDbContext.CurrencyRates.Add(rate);
            else
                existing.Rate = rate.Rate;
        }

        await _applicationDbContext.SaveChangesAsync();
    }
}
