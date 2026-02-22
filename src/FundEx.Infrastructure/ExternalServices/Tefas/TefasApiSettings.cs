namespace FundEx.Infrastructure.ExternalServices.Tefas;

public class TefasApiSettings
{
    public const string SectionName = "TefasApi";
    public string BaseUrl { get; set; } = "https://tefas.takasbank.com.tr/api/funds/";
    public int RequestDelayMs { get; set; } = 300;
    public int PageSize { get; set; } = 500;
    public int BackfillYears { get; set; } = 5;
}
