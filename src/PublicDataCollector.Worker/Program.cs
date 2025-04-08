using Hangfire;
using Hangfire.SqlServer;
using Microsoft.EntityFrameworkCore;
using PublicDataCollector.Application.Data;
using PublicDataCollector.Application.Gateway;
using PublicDataCollector.Domain.Repositories;
using PublicDataCollector.Infrastructure.Repositories;
using PublicDataCollector.Jobs;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.ClearProviders();
builder.Host.UseSerilog((context, configuration) => configuration.ReadFrom.Configuration(context.Configuration));

var connectionString = builder.Configuration.GetConnectionString("Default");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddScoped<ICurrencyRateRepository, CurrencyRateRepository>();
builder.Services.AddScoped<IExchangeRateApiClient, ExchangeRateApiClient>();

builder.Services.AddHttpClient("clientProvider", (provider, client) =>
{
    client.BaseAddress = new Uri($"https://v6.exchangerate-api.com/v6/{builder.Configuration["ApiKey"]}/latest/");
});

builder.Services.AddScoped<JobScheduler>();

builder.Services.AddHangfire(configuration => configuration
       .UseSimpleAssemblyNameTypeSerializer()
       .UseRecommendedSerializerSettings()
       .UseSqlServerStorage(connectionString,
           new SqlServerStorageOptions
           {
               CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
               SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
               QueuePollInterval = TimeSpan.Zero,
               UseRecommendedIsolationLevel = true,
               DisableGlobalLocks = true
           }));

builder.Services.AddHangfireServer();

var app = builder.Build();

app.UseSerilogRequestLogging();

using (var scope = app.Services.CreateScope())
{
    var jobScheduler = scope.ServiceProvider.GetRequiredService<JobScheduler>();
    jobScheduler.RegisterJobs();
}

app.Run();