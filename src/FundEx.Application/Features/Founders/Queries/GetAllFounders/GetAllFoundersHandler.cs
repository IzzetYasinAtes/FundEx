namespace FundEx.Application.Features.Founders.Queries.GetAllFounders;
using FundEx.Application.Common.Interfaces;
using FundEx.Domain.Entities;
using MediatR;

public class GetAllFoundersHandler : IRequestHandler<GetAllFoundersQuery, GetAllFoundersResponse>
{
    private readonly IRepository<Founder> _repository;

    public GetAllFoundersHandler(IRepository<Founder> repository) => _repository = repository;

    public async Task<GetAllFoundersResponse> Handle(GetAllFoundersQuery request, CancellationToken cancellationToken)
    {
        var query = _repository.Query();

        if (!string.IsNullOrEmpty(request.FundTypeCode))
            query = query.Where(f => f.FundTypeCode == request.FundTypeCode);

        var founders = query
            .OrderBy(f => f.Title)
            .Select(f => new FounderDto
            {
                Id = f.Id,
                Code = f.Code,
                Title = f.Title,
                FundTypeCode = f.FundTypeCode
            })
            .ToList();

        return new GetAllFoundersResponse { Founders = founders };
    }
}
