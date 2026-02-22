namespace FundEx.Infrastructure.ExternalServices.Tefas;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using FundEx.Application.Common.Interfaces;
using FundEx.Application.Common.Models.Tefas;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

public class TefasApiService : ITefasApiService
{
    private readonly HttpClient _httpClient;
    private readonly TefasApiSettings _settings;
    private readonly ILogger<TefasApiService> _logger;

    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        PropertyNameCaseInsensitive = true
    };

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
            var jsonBody = JsonSerializer.Serialize(body, JsonOptions);
            _logger.LogDebug("TEFAS API call: {Endpoint} Body: {Body}", endpoint, jsonBody);

            var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(endpoint, content, ct);

            var responseBody = await response.Content.ReadAsStringAsync(ct);

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogWarning("TEFAS API {Endpoint} returned {StatusCode}: {Response}",
                    endpoint, (int)response.StatusCode, responseBody);
                return default;
            }

            await Task.Delay(_settings.RequestDelayMs, ct);
            return JsonSerializer.Deserialize<T>(responseBody, JsonOptions);
        }
        catch (TaskCanceledException)
        {
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "TEFAS API error: {Endpoint}", endpoint);
            return default;
        }
    }
}
