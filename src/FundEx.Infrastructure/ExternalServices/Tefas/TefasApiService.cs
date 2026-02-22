namespace FundEx.Infrastructure.ExternalServices.Tefas;
using System.Net.Http.Json;
using FundEx.Application.Common.Interfaces;
using FundEx.Application.Common.Models.Tefas;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

public class TefasApiService : ITefasApiService
{
    private readonly HttpClient _httpClient;
    private readonly TefasApiSettings _settings;
    private readonly ILogger<TefasApiService> _logger;

    public TefasApiService(HttpClient httpClient, IOptions<TefasApiSettings> settings, ILogger<TefasApiService> logger)
    {
        _httpClient = httpClient;
        _settings = settings.Value;
        _logger = logger;
    }

    public async Task<List<FonTurResult>> GetFundTypesAsync(string language = "TR", CancellationToken ct = default)
    {
        var response = await PostAsync<TefasBaseResponse<FonTurResult>>("fonTurGetir", new { dil = language, flag = 1 }, ct);
        return response?.ResultList ?? [];
    }

    public async Task<List<FonUnvanResult>> GetFundTitlesAsync(string fundTypeCode, string language = "TR", CancellationToken ct = default)
    {
        var response = await PostAsync<TefasBaseResponse<FonUnvanResult>>("fonUnvanGetir", new { dil = language, tur = fundTypeCode }, ct);
        return response?.ResultList ?? [];
    }

    public async Task<List<FonGrupResult>> GetFundGroupsAsync(string language = "TR", CancellationToken ct = default)
    {
        var response = await PostAsync<TefasBaseResponse<FonGrupResult>>("fonGrupGetir", new { dil = language }, ct);
        return response?.ResultList ?? [];
    }

    public async Task<List<FonDetayResult>> GetFundDetailsAsync(string fundTypeCode, string language = "TR", CancellationToken ct = default)
    {
        var response = await PostAsync<TefasBaseResponse<FonDetayResult>>("fonDetayGetir", new { fonTipi = fundTypeCode, dil = language }, ct);
        return response?.ResultList ?? [];
    }

    public async Task<List<FonKurucuResult>> GetFoundersAsync(string fundTypeCode, string language = "TR", CancellationToken ct = default)
    {
        var response = await PostAsync<TefasBaseResponse<FonKurucuResult>>("fonKurucuGetir", new { fonTipi = fundTypeCode, dil = language }, ct);
        return response?.ResultList ?? [];
    }

    public async Task<FonGnlBlgResponse> GetFundGeneralInfoAsync(FonGnlBlgRequest request, CancellationToken ct = default)
    {
        var response = await PostAsync<FonGnlBlgResponse>("fonGnlBlgSiraliGetir", request, ct);
        return response ?? new FonGnlBlgResponse();
    }

    public async Task<DagilimResponse> GetFundDistributionAsync(DagilimRequest request, CancellationToken ct = default)
    {
        var response = await PostAsync<DagilimResponse>("dagilimSiraliGetirT", request, ct);
        return response ?? new DagilimResponse();
    }

    private async Task<T?> PostAsync<T>(string endpoint, object body, CancellationToken ct)
    {
        try
        {
            _logger.LogDebug("TEFAS API call: {Endpoint}", endpoint);
            var response = await _httpClient.PostAsJsonAsync(endpoint, body, ct);
            response.EnsureSuccessStatusCode();
            await Task.Delay(_settings.RequestDelayMs, ct);
            return await response.Content.ReadFromJsonAsync<T>(ct);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "TEFAS API error: {Endpoint}", endpoint);
            throw;
        }
    }
}
