namespace FundEx.Application.Features.Funds.Queries.GetAllFunds;
using MediatR;

public record GetAllFundsQuery(int PageNumber = 1, int PageSize = 25, string? FundTypeCode = null, string? SearchText = null)
    : IRequest<GetAllFundsResponse>;
