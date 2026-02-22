namespace FundEx.Application.Features.FundDailyData.Queries.GetFundDailyData;
using FundEx.Application.Common.Interfaces;
using MediatR;

public record GetFundDailyDataQuery(
    string FundCode,
    DateTime StartDate,
    DateTime EndDate,
    int PageNumber = 1,
    int PageSize = 25
) : IRequest<GetFundDailyDataResponse>, ICacheable
{
    public string CacheKey => $"FundDailyData:{FundCode}:{StartDate:yyyyMMdd}:{EndDate:yyyyMMdd}:{PageNumber}:{PageSize}";
    public TimeSpan? CacheDuration => TimeSpan.FromMinutes(10);
}
