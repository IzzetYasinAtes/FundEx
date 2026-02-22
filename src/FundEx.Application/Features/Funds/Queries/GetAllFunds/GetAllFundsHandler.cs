namespace FundEx.Application.Features.Funds.Queries.GetAllFunds;
using FundEx.Application.Common.Interfaces;
using FundEx.Application.Common.Models;
using FundEx.Domain.Entities;
using Mapster;
using MediatR;
using System.Linq;

public class GetAllFundsHandler : IRequestHandler<GetAllFundsQuery, GetAllFundsResponse>
{
    private readonly IRepository<Fund> _fundRepository;

    public GetAllFundsHandler(IRepository<Fund> fundRepository) => _fundRepository = fundRepository;

    public async Task<GetAllFundsResponse> Handle(GetAllFundsQuery request, CancellationToken cancellationToken)
    {
        var query = _fundRepository.Query();

        if (!string.IsNullOrEmpty(request.FundTypeCode))
        {
            query = query.Where(f => f.FundType.Code == request.FundTypeCode);
        }

        if (!string.IsNullOrEmpty(request.SearchText))
        {
            query = query.Where(f => f.Code.Contains(request.SearchText) || f.Title.Contains(request.SearchText));
        }

        var totalCount = query.Count();
        var items = query
            .OrderBy(f => f.Code)
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
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
            .ToList();

        return new GetAllFundsResponse
        {
            Result = new PagedResult<FundDto>
            {
                Items = items,
                TotalCount = totalCount,
                PageNumber = request.PageNumber,
                PageSize = request.PageSize
            }
        };
    }
}
