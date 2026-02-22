namespace FundEx.Application.Features.FundDailyData.Queries.GetFundDailyData;
using FundEx.Application.Common.Interfaces;
using FundEx.Application.Common.Models;
using MediatR;

public class GetFundDailyDataHandler : IRequestHandler<GetFundDailyDataQuery, GetFundDailyDataResponse>
{
    private readonly IRepository<Domain.Entities.FundDailyData> _repository;

    public GetFundDailyDataHandler(IRepository<Domain.Entities.FundDailyData> repository) => _repository = repository;

    public async Task<GetFundDailyDataResponse> Handle(GetFundDailyDataQuery request, CancellationToken cancellationToken)
    {
        var query = _repository.Query()
            .Where(d => d.Fund.Code == request.FundCode && d.Date >= request.StartDate && d.Date <= request.EndDate);

        var totalCount = query.Count();
        var items = query
            .OrderByDescending(d => d.Date)
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(d => new FundDailyDataDto
            {
                Id = d.Id,
                FundCode = d.Fund.Code,
                Date = d.Date,
                Price = d.Price,
                ShareCount = d.ShareCount,
                InvestorCount = d.InvestorCount,
                PortfolioSize = d.PortfolioSize,
                ExchangeBulletinPrice = d.ExchangeBulletinPrice,
                Stock = d.Stock,
                GovernmentBond = d.GovernmentBond,
                CommercialPaper = d.CommercialPaper,
                CorporateBond = d.CorporateBond,
                ReverseRepo = d.ReverseRepo,
                PreciousMetals = d.PreciousMetals,
                FuturesCashCollateral = d.FuturesCashCollateral,
                InvestmentFundShares = d.InvestmentFundShares,
                DepositTl = d.DepositTl,
                DepositFx = d.DepositFx,
                ParticipationAccountFx = d.ParticipationAccountFx,
                ParticipationAccountTl = d.ParticipationAccountTl,
                ForeignEtf = d.ForeignEtf,
                ForeignStock = d.ForeignStock,
                Other = d.Other
            })
            .ToList();

        return new GetFundDailyDataResponse
        {
            Result = new PagedResult<FundDailyDataDto>
            {
                Items = items,
                TotalCount = totalCount,
                PageNumber = request.PageNumber,
                PageSize = request.PageSize
            }
        };
    }
}
