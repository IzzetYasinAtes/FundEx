namespace FundEx.WebApi.Controllers;
using FundEx.Infrastructure.BackgroundServices;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class DataSyncController : ControllerBase
{
    private readonly DataSyncOrchestrator _orchestrator;

    public DataSyncController(DataSyncOrchestrator orchestrator) => _orchestrator = orchestrator;

    [HttpPost("trigger")]
    public async Task<IActionResult> TriggerSync([FromQuery] DateTime? startDate, [FromQuery] DateTime? endDate, CancellationToken ct)
    {
        if (startDate.HasValue && endDate.HasValue)
        {
            await _orchestrator.SyncDailyDataAsync(startDate.Value, endDate.Value, ct);
        }
        else
        {
            await _orchestrator.RunDailySyncAsync(ct);
        }

        return Ok(new { Message = "Sync completed", IsSuccess = true });
    }
}
