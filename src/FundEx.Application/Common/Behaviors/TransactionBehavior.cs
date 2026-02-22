namespace FundEx.Application.Common.Behaviors;
using FundEx.Application.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

public class TransactionBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<TransactionBehavior<TRequest, TResponse>> _logger;

    public TransactionBehavior(IUnitOfWork unitOfWork, ILogger<TransactionBehavior<TRequest, TResponse>> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        if (request is not ITransactional)
        {
            return await next(cancellationToken);
        }

        _logger.LogInformation("Begin transaction for {RequestName}", typeof(TRequest).Name);
        var response = await next(cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        _logger.LogInformation("Committed transaction for {RequestName}", typeof(TRequest).Name);
        return response;
    }
}
