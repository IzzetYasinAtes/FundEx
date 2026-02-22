namespace FundEx.Application.Common.Interfaces;
using FundEx.Application.Common.Models.Tefas;

public interface ITefasApiService
{
    Task<List<FonTurResult>> GetFundTypesAsync(string language = "TR", CancellationToken ct = default);
    Task<List<FonUnvanResult>> GetFundTitlesAsync(string fundTypeCode, string language = "TR", CancellationToken ct = default);
    Task<List<FonGrupResult>> GetFundGroupsAsync(string language = "TR", CancellationToken ct = default);
    Task<List<FonDetayResult>> GetFundDetailsAsync(string fundTypeCode, string language = "TR", CancellationToken ct = default);
    Task<List<FonKurucuResult>> GetFoundersAsync(string fundTypeCode, string language = "TR", CancellationToken ct = default);
    Task<FonGnlBlgResponse> GetFundGeneralInfoAsync(FonGnlBlgRequest request, CancellationToken ct = default);
    Task<DagilimResponse> GetFundDistributionAsync(DagilimRequest request, CancellationToken ct = default);
}
