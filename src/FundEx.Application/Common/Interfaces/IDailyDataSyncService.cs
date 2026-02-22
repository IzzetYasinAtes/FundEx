namespace FundEx.Application.Common.Interfaces;

public interface IDailyDataSyncService
{
    Task SyncAsync(string fundTypeCode, string startDate, string endDate, CancellationToken cancellationToken = default);
}
