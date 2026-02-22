namespace FundEx.Application.Common.Interfaces;

public interface IMetadataSyncService
{
    Task SyncAsync(string fundTypeCode, CancellationToken cancellationToken = default);
}
