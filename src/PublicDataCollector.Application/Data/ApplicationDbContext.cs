using Microsoft.EntityFrameworkCore;
using PublicDataCollector.Domain.Entities;

namespace PublicDataCollector.Application.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options) { }

    public DbSet<CurrencyRate> CurrencyRates => Set<CurrencyRate>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CurrencyRate>()
            .HasIndex(x => new { x.TargetCurrency, x.Date });
    }
}
