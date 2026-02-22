namespace FundEx.Application.Features.DataSync.Commands.TriggerDataSync;
using MediatR;

public record TriggerDataSyncCommand(DateTime? StartDate = null, DateTime? EndDate = null) : IRequest<TriggerDataSyncResponse>;
