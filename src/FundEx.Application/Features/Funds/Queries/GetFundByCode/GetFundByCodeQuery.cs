namespace FundEx.Application.Features.Funds.Queries.GetFundByCode;
using MediatR;

public record GetFundByCodeQuery(string Code) : IRequest<GetFundByCodeResponse>;
