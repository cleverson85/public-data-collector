namespace PublicDataCollector.Domain.Entities;

public sealed class CurrencyRate : BaseEntity
{
    public string TargetCurrency { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
    public decimal Rate { get; set; }
    public DateTime Date { get; set; } = DateTime.Now.Date;
}
