namespace FundEx.Application.Common.Models;
public class DynamicQuery
{
    public IEnumerable<Sort>? Sort { get; set; }
    public Filter? Filter { get; set; }
}
public class Sort
{
    public string Field { get; set; } = null!;
    public string Dir { get; set; } = "asc";
}
public class Filter
{
    public string Field { get; set; } = null!;
    public string Operator { get; set; } = null!;
    public string? Value { get; set; }
    public string? Logic { get; set; }
    public IEnumerable<Filter>? Filters { get; set; }
}
