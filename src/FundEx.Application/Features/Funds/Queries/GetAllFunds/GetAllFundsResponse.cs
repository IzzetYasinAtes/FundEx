namespace FundEx.Application.Features.Funds.Queries.GetAllFunds;
using FundEx.Application.Common.Models;

public class GetAllFundsResponse
{
    public PagedResult<FundDto> Result { get; set; } = null!;
}

public class FundDto
{
    public Guid Id { get; set; }
    public string Code { get; set; } = null!;
    public string Title { get; set; } = null!;
    public string FundTypeCode { get; set; } = null!;
    public string FundTypeName { get; set; } = null!;
    public string? FounderCode { get; set; }
    public string? FounderTitle { get; set; }
}
