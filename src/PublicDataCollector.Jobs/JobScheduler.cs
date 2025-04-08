using Hangfire;
using PublicDataCollector.Jobs.Jobs;

namespace PublicDataCollector.Jobs;

public class JobScheduler
{
    private readonly IRecurringJobManager _recurringJobs;

    public JobScheduler(IRecurringJobManager recurringJobs)
    {
        _recurringJobs = recurringJobs;
    }

    public void RegisterJobs()
    {
        _recurringJobs.AddOrUpdate<CurrencyRateUpdaterJob>(
            "update-currency-rates",
            job => job.ExecuteAsync(),
            Cron.Hourly
        );
    }
}
