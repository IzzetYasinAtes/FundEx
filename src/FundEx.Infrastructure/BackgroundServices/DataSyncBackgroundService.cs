namespace FundEx.Infrastructure.BackgroundServices;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

public class DataSyncBackgroundService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<DataSyncBackgroundService> _logger;

    public DataSyncBackgroundService(IServiceProvider serviceProvider, ILogger<DataSyncBackgroundService> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try
        {
            using var scope = _serviceProvider.CreateScope();
            var orchestrator = scope.ServiceProvider.GetRequiredService<DataSyncOrchestrator>();
            await orchestrator.RunFullSyncAsync(stoppingToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Initial data sync failed");
        }

        while (!stoppingToken.IsCancellationRequested)
        {
            var now = DateTime.UtcNow;
            var nextMidnight = now.Date.AddDays(1);
            var delay = nextMidnight - now;

            _logger.LogInformation("Next daily sync scheduled at {NextRun} (in {Delay})", nextMidnight, delay);
            await Task.Delay(delay, stoppingToken);

            try
            {
                using var scope = _serviceProvider.CreateScope();
                var orchestrator = scope.ServiceProvider.GetRequiredService<DataSyncOrchestrator>();
                await orchestrator.RunDailySyncAsync(stoppingToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Daily sync failed");
            }
        }
    }
}
