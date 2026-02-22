namespace FundEx.Infrastructure.BackgroundServices;

using FundEx.Application.Common.Constants;
using FundEx.Application.Common.Interfaces;
using FundEx.Infrastructure.ExternalServices.Tefas;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

public sealed class DataSyncOrchestrator
{
    private readonly IServiceProvider _serviceProvider;
    private readonly TefasApiSettings _settings;
    private readonly ILogger<DataSyncOrchestrator> _logger;

    public DataSyncOrchestrator(
        IServiceProvider serviceProvider,
        IOptions<TefasApiSettings> settings,
        ILogger<DataSyncOrchestrator> logger)
    {
        _serviceProvider = serviceProvider;
        _settings = settings.Value;
        _logger = logger;
    }

    public async Task RunFullSyncAsync(CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Starting full data sync");
        await SyncMetadataAsync(cancellationToken);

        var endDate = DateTime.UtcNow.Date;
        var startDate = endDate.AddYears(-_settings.BackfillYears);
        await SyncDailyDataAsync(startDate, endDate, cancellationToken);
        _logger.LogInformation("Full data sync completed");
    }

    public async Task RunDailySyncAsync(CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Starting daily sync");
        await SyncMetadataAsync(cancellationToken);

        var today = DateTime.UtcNow.Date;
        await SyncDailyDataAsync(today.AddDays(-1), today, cancellationToken);
        _logger.LogInformation("Daily sync completed");
    }

    public async Task SyncDailyDataAsync(DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Syncing daily data from {Start:yyyy-MM-dd} to {End:yyyy-MM-dd}", startDate, endDate);

        var currentStart = startDate;
        while (currentStart < endDate)
        {
            var currentEnd = currentStart.AddMonths(1) > endDate ? endDate : currentStart.AddMonths(1);
            var startStr = currentStart.ToString("yyyyMMdd");
            var endStr = currentEnd.ToString("yyyyMMdd");

            await ExecutePerFundTypeAsync(async (fundTypeCode, ct) =>
            {
                using var scope = _serviceProvider.CreateScope();
                var service = scope.ServiceProvider.GetRequiredService<IDailyDataSyncService>();
                await service.SyncAsync(fundTypeCode, startStr, endStr, ct);
            }, cancellationToken);

            _logger.LogInformation("Completed period {Start} to {End}", startStr, endStr);
            currentStart = currentEnd.AddDays(1);
        }
    }

    private async Task SyncMetadataAsync(CancellationToken ct)
    {
        await ExecutePerFundTypeAsync(async (fundTypeCode, cancellation) =>
        {
            using var scope = _serviceProvider.CreateScope();
            var service = scope.ServiceProvider.GetRequiredService<IMetadataSyncService>();
            await service.SyncAsync(fundTypeCode, cancellation);
        }, ct);
    }

    private async Task ExecutePerFundTypeAsync(Func<string, CancellationToken, Task> action, CancellationToken ct)
    {
        foreach (var fundTypeCode in DataSyncConstants.FundTypeCodes)
        {
            try
            {
                await action(fundTypeCode, ct);
            }
            catch (OperationCanceledException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Sync failed for {FundType}", fundTypeCode);
            }
        }
    }
}
