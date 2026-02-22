namespace FundEx.Application.Features.Founders.Queries.GetAllFounders;

public class GetAllFoundersResponse
{
    public IReadOnlyList<FounderDto> Founders { get; set; } = [];
}

public class FounderDto
{
    public Guid Id { get; set; }
    public string Code { get; set; } = null!;
    public string Title { get; set; } = null!;
    public string FundTypeCode { get; set; } = null!;
}
