namespace FundEx.Application.Features.Funds.Queries.GetFundByCode;
using FundEx.Application.Common.Exceptions;
using FundEx.Application.Common.Interfaces;
using FundEx.Application.Features.Funds.Queries.GetAllFunds;
using FundEx.Domain.Entities;
using MediatR;

public class GetFundByCodeHandler : IRequestHandler<GetFundByCodeQuery, GetFundByCodeResponse>
{
    private readonly IRepository<Fund> _fundRepository;

    public GetFundByCodeHandler(IRepository<Fund> fundRepository) => _fundRepository = fundRepository;

    public async Task<GetFundByCodeResponse> Handle(GetFundByCodeQuery request, CancellationToken cancellationToken)
    {
        var fund = _fundRepository.Query()
            .Where(f => f.Code == request.Code)
            .Select(f => new FundDto
            {
                Id = f.Id,
                Code = f.Code,
                Title = f.Title,
                FundTypeCode = f.FundType.Code,
                FundTypeName = f.FundType.Name,
                FounderCode = f.Founder != null ? f.Founder.Code : null,
                FounderTitle = f.Founder != null ? f.Founder.Title : null
            })
            .FirstOrDefault();

        if (fund is null)
            throw new NotFoundException(nameof(Fund), request.Code);

        return new GetFundByCodeResponse { Fund = fund };
    }
}
