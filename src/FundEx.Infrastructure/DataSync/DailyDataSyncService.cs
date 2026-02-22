namespace FundEx.Infrastructure.DataSync;

using FundEx.Application.Common.Interfaces;
using FundEx.Application.Common.Models.Tefas;
using FundEx.Domain.Entities;
using FundEx.Infrastructure.ExternalServices.Tefas;
using Mapster;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

public sealed class DailyDataSyncService : IDailyDataSyncService
{
    private readonly ITefasApiService _tefasApi;
    private readonly IRepository<Fund> _fundRepository;
    private readonly IRepository<FundType> _fundTypeRepository;
    private readonly IRepository<FundDailyData> _dailyDataRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly int _pageSize;
    private readonly ILogger<DailyDataSyncService> _logger;

    public DailyDataSyncService(
        ITefasApiService tefasApi,
        IRepository<Fund> fundRepository,
        IRepository<FundType> fundTypeRepository,
        IRepository<FundDailyData> dailyDataRepository,
        IUnitOfWork unitOfWork,
        IOptions<TefasApiSettings> settings,
        ILogger<DailyDataSyncService> logger)
    {
        _tefasApi = tefasApi;
        _fundRepository = fundRepository;
        _fundTypeRepository = fundTypeRepository;
        _dailyDataRepository = dailyDataRepository;
        _unitOfWork = unitOfWork;
        _pageSize = settings.Value.PageSize;
        _logger = logger;
    }

    public async Task SyncAsync(string fundTypeCode, string startDate, string endDate, CancellationToken cancellationToken = default)
    {
        var fundType = await _fundTypeRepository.FirstOrDefaultAsync(ft => ft.Code == fundTypeCode, cancellationToken);
        if (fundType is null)
        {
            return;
        }

        var page = 1;
        while (true)
        {
            var generalInfo = await _tefasApi.GetFundGeneralInfoAsync(
                CreateGeneralInfoRequest(fundTypeCode, startDate, endDate, page, _pageSize), cancellationToken);

            if (ShouldAbort(generalInfo, fundTypeCode, startDate, endDate))
            {
                break;
            }

            var distributionMap = await GetDistributionMapAsync(fundTypeCode, startDate, endDate, page, cancellationToken);
            var processed = await UpsertPageAsync(fundType, generalInfo.ResultList!, distributionMap, cancellationToken);

            await _unitOfWork.SaveChangesAsync(cancellationToken);
            _logger.LogInformation("Synced page {Page} for {Type} ({Start}-{End}), {Count} records", page, fundTypeCode, startDate, endDate, processed);

            if (!HasMorePages(generalInfo, page))
            {
                break;
            }
            page++;
        }
    }

    private static FonGnlBlgRequest CreateGeneralInfoRequest(string fundTypeCode, string startDate, string endDate, int page, int pageSize)
    {
        var (basSira, bitSira) = GetPageRange(page, pageSize);
        return new FonGnlBlgRequest
        {
            FonTipi = fundTypeCode,
            BasTarih = startDate,
            BitTarih = endDate,
            BasSira = basSira,
            BitSira = bitSira
        };
    }

    private static (int BasSira, int BitSira) GetPageRange(int page, int pageSize) =>
        ((page - 1) * pageSize + 1, page * pageSize);

    private bool ShouldAbort(FonGnlBlgResponse response, string fundTypeCode, string startDate, string endDate)
    {
        if (response.ResultList is { Count: > 0 })
        {
            return false;
        }
        if (!string.IsNullOrEmpty(response.ErrorMessage))
        {
            _logger.LogWarning("TEFAS API: {Type} ({Start}-{End}) - {Error}", fundTypeCode, startDate, endDate, response.ErrorMessage);
        }
        return true;
    }

    private async Task<Dictionary<(string FonKodu, string Tarih), DagilimResult>> GetDistributionMapAsync(
        string fundTypeCode, string startDate, string endDate, int page, CancellationToken ct)
    {
        var (basSira, bitSira) = GetPageRange(page, _pageSize);
        var request = new DagilimRequest
        {
            FonTipi = fundTypeCode,
            BasTarih = startDate,
            BitTarih = endDate,
            BasSira = basSira,
            BitSira = bitSira
        };
        var response = await _tefasApi.GetFundDistributionAsync(request, ct);
        var list = response.ResultList ?? [];
        return list.GroupBy(d => (d.FonKodu, d.Tarih)).ToDictionary(g => g.Key, g => g.First());
    }

    private async Task<int> UpsertPageAsync(FundType fundType, List<FonGnlBlgResult> items,
        Dictionary<(string FonKodu, string Tarih), DagilimResult> distributionMap, CancellationToken ct)
    {
        var count = 0;
        foreach (var info in items)
        {
            var fund = await GetOrCreateFundAsync(fundType, info, ct);
            if (fund is null)
            {
                continue;
            }
            if (!DateTime.TryParse(info.Tarih, out var date))
            {
                continue;
            }

            var existing = await _dailyDataRepository.FirstOrDefaultAsync(d => d.FundId == fund.Id && d.Date == date, ct);
            var dailyData = existing ?? CreateNewDailyData(fund.Id, date);

            ApplyData(dailyData, info, distributionMap);

            if (existing is null)
            {
                await _dailyDataRepository.AddAsync(dailyData, ct);
            }
            else
            {
                _dailyDataRepository.Update(dailyData);
            }

            count++;
        }
        return count;
    }

    private async Task<Fund?> GetOrCreateFundAsync(FundType fundType, FonGnlBlgResult info, CancellationToken ct)
    {
        var fund = await _fundRepository.FirstOrDefaultAsync(f => f.Code == info.FonKodu, ct);
        if (fund is not null)
        {
            return fund;
        }

        fund = new Fund { Id = Guid.NewGuid(), Code = info.FonKodu, Title = info.FonUnvan, FundTypeId = fundType.Id };
        await _fundRepository.AddAsync(fund, ct);
        await _unitOfWork.SaveChangesAsync(ct);
        return fund;
    }

    private static FundDailyData CreateNewDailyData(Guid fundId, DateTime date) =>
        new() { Id = Guid.NewGuid(), FundId = fundId, Date = date };

    private static void ApplyData(FundDailyData target, FonGnlBlgResult info,
        Dictionary<(string FonKodu, string Tarih), DagilimResult> distributionMap)
    {
        target.Price = info.Fiyat;
        target.ShareCount = info.TedPaySayisi;
        target.InvestorCount = info.KisiSayisi;
        target.PortfolioSize = info.PortfoyBuyukluk;
        target.ExchangeBulletinPrice = info.BorsaBultenFiyat;

        if (distributionMap.TryGetValue((info.FonKodu, info.Tarih), out var dist))
        {
            dist.Adapt(target);
        }
    }

    private static bool HasMorePages(FonGnlBlgResponse response, int currentPage) =>
        response.ToplamSayfa is { } total && currentPage < total;
}
