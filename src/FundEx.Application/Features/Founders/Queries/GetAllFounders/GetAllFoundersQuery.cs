namespace FundEx.Application.Features.Founders.Queries.GetAllFounders;
using FundEx.Application.Common.Interfaces;
using MediatR;

public record GetAllFoundersQuery(string? FundTypeCode = null) : IRequest<GetAllFoundersResponse>, ICacheable
{
    public string CacheKey => $"Founders:{FundTypeCode ?? "ALL"}";
    public TimeSpan? CacheDuration => TimeSpan.FromHours(1);
}
