using Microsoft.EntityFrameworkCore;
using PublicDataCollector.Application.Data;
using PublicDataCollector.Domain.Entities;

namespace PublicDataCollector.Application.CurrencyRates;

public sealed class GetCurrencyRatesService : IGetCurrencyRatesService
{
    private readonly ApplicationDbContext _applicationDbContext;

    public GetCurrencyRatesService(ApplicationDbContext applicationDbContext)
    {
        _applicationDbContext = applicationDbContext;
    }

    public async Task<IList<CurrencyRate>> GetAllRatesAsync(CancellationToken cancellationToken)
    {
        return await _applicationDbContext.CurrencyRates
            .OrderBy(c => c.Code)
            .ThenBy(c => c.Date)
            .ToListAsync(cancellationToken);
    }

    public async Task<RatesFilterResponse> GetRatesByFilterAsync(string? code, DateTime? date, int page, int pageSize, CancellationToken cancellationToken)
    {
        var allRates = await GetAllRatesAsync(cancellationToken);
        var query = allRates.AsQueryable();

        if (!string.IsNullOrEmpty(code))
            query = query.Where(c => c.Code.Contains(code, StringComparison.CurrentCultureIgnoreCase));

        if (date.HasValue)
            query = query.Where(c => c.Date.Date == date.Value.Date);

        var total = query.Count();
        var results = query
            .OrderBy(x => x.Code)
            .ThenByDescending(c => c.Date)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        return new RatesFilterResponse(total, page, pageSize, results);
    }
}

public interface IGetCurrencyRatesService
{
    Task<IList<CurrencyRate>> GetAllRatesAsync(CancellationToken cancellationToken);
    Task<RatesFilterResponse> GetRatesByFilterAsync(string? code, DateTime? date, int page, int pageSize, CancellationToken cancellationToken);
}

public record RatesFilterResponse(int Total, int Page, int PageSize, IList<CurrencyRate> CurrencyRates);