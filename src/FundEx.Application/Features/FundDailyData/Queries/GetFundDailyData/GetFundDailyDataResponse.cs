namespace FundEx.Application.Features.FundDailyData.Queries.GetFundDailyData;
using FundEx.Application.Common.Models;

public class GetFundDailyDataResponse
{
    public PagedResult<FundDailyDataDto> Result { get; set; } = null!;
}

public class FundDailyDataDto
{
    public Guid Id { get; set; }
    public string FundCode { get; set; } = null!;
    public DateTime Date { get; set; }
    public decimal Price { get; set; }
    public long ShareCount { get; set; }
    public int InvestorCount { get; set; }
    public decimal PortfolioSize { get; set; }
    public decimal? ExchangeBulletinPrice { get; set; }
    public decimal? Stock { get; set; }
    public decimal? GovernmentBond { get; set; }
    public decimal? CommercialPaper { get; set; }
    public decimal? CorporateBond { get; set; }
    public decimal? ReverseRepo { get; set; }
    public decimal? PreciousMetals { get; set; }
    public decimal? FuturesCashCollateral { get; set; }
    public decimal? InvestmentFundShares { get; set; }
    public decimal? DepositTl { get; set; }
    public decimal? DepositFx { get; set; }
    public decimal? ParticipationAccountFx { get; set; }
    public decimal? ParticipationAccountTl { get; set; }
    public decimal? ForeignEtf { get; set; }
    public decimal? ForeignStock { get; set; }
    public decimal? Other { get; set; }
}
