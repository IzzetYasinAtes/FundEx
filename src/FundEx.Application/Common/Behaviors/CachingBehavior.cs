namespace FundEx.Application.Common.Behaviors;
using FundEx.Application.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

public class CachingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    private readonly ICacheService _cache;
    private readonly ILogger<CachingBehavior<TRequest, TResponse>> _logger;

    public CachingBehavior(ICacheService cache, ILogger<CachingBehavior<TRequest, TResponse>> logger)
    {
        _cache = cache;
        _logger = logger;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        if (request is not ICacheable cacheable)
        {
            return await next(cancellationToken);
        }

        var cached = await _cache.GetAsync<TResponse>(cacheable.CacheKey, cancellationToken);
        if (cached is not null)
        {
            _logger.LogInformation("Cache hit for {CacheKey}", cacheable.CacheKey);
            return cached;
        }

        var response = await next(cancellationToken);
        await _cache.SetAsync(cacheable.CacheKey, response, cacheable.CacheDuration, cancellationToken);
        _logger.LogInformation("Cache set for {CacheKey}", cacheable.CacheKey);
        return response;
    }
}
