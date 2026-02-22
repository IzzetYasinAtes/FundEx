namespace FundEx.Application.Common.Interfaces;
public interface ICacheable
{
    string CacheKey { get; }
    TimeSpan? CacheDuration => TimeSpan.FromMinutes(5);
}
