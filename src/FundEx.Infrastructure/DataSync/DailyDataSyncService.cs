namespace FundEx.Infrastructure.DataSync;

using FundEx.Application.Common.Interfaces;
using FundEx.Application.Common.Models.Tefas;
using FundEx.Infrastructure.ExternalServices.Tefas;
using FundEx.Domain.Entities;
using Mapster;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

public sealed class DailyDataSyncService(
    ITefasApiService tefasApi,
    IRepository<Fund> fundRepository,
    IRepository<FundType> fundTypeRepository,
    IRepository<FundDailyData> dailyDataRepository,
    IUnitOfWork unitOfWork,
    IOptions<TefasApiSettings> settings,
    ILogger<DailyDataSyncService> logger) : IDailyDataSyncService
{
    private readonly TefasApiSettings _settings = settings.Value;

    public async Task SyncAsync(string fundTypeCode, string startDate, string endDate, CancellationToken cancellationToken = default)
    {
        var fundType = await fundTypeRepository.FirstOrDefaultAsync(ft => ft.Code == fundTypeCode, cancellationToken);
        if (fundType is null) return;

        var pageSize = _settings.PageSize;
        var page = 1;

        while (true)
        {
            var generalInfo = await FetchGeneralInfoAsync(fundTypeCode, startDate, endDate, page, pageSize, cancellationToken);
            if (generalInfo.ResultList.Count == 0) break;

            var distributionMap = await FetchDistributionMapAsync(fundTypeCode, startDate, endDate, page, pageSize, cancellationToken);

            var saved = await ProcessPageAsync(fundType, generalInfo.ResultList, distributionMap, cancellationToken);
            await unitOfWork.SaveChangesAsync(cancellationToken);

            logger.LogInformation("Synced page {Page} for {Type} ({Start}-{End}), {Count} records",
                page, fundTypeCode, startDate, endDate, saved);

            if (!HasMorePages(generalInfo, page)) break;
            page++;
        }
    }

    private async Task<FonGnlBlgResponse> FetchGeneralInfoAsync(string fundTypeCode, string startDate, string endDate, int page, int pageSize, CancellationToken ct)
    {
        var request = new FonGnlBlgRequest
        {
            FonTipi = fundTypeCode,
            BasTarih = startDate,
            BitTarih = endDate,
            BasSira = (page - 1) * pageSize + 1,
            BitSira = page * pageSize
        };
        return await tefasApi.GetFundGeneralInfoAsync(request, ct);
    }

    private async Task<Dictionary<(string FonKodu, string Tarih), DagilimResult>> FetchDistributionMapAsync(
        string fundTypeCode, string startDate, string endDate, int page, int pageSize, CancellationToken ct)
    {
        var request = new DagilimRequest
        {
            FonTipi = fundTypeCode,
            BasTarih = startDate,
            BitTarih = endDate,
            BasSira = (page - 1) * pageSize + 1,
            BitSira = page * pageSize
        };
        var response = await tefasApi.GetFundDistributionAsync(request, ct);
        return response.ResultList
            .GroupBy(d => (d.FonKodu, d.Tarih))
            .ToDictionary(g => g.Key, g => g.First());
    }

    private async Task<int> ProcessPageAsync(FundType fundType, List<FonGnlBlgResult> generalList,
        Dictionary<(string FonKodu, string Tarih), DagilimResult> distributionMap, CancellationToken ct)
    {
        var count = 0;
        foreach (var info in generalList)
        {
            var fund = await EnsureFundExistsAsync(fundType, info, ct);
            if (fund is null) continue;

            if (!DateTime.TryParse(info.Tarih, out var date)) continue;
            if (await dailyDataRepository.AnyAsync(d => d.FundId == fund.Id && d.Date == date, ct)) continue;

            var dailyData = BuildDailyData(fund.Id, info, date, distributionMap);
            await dailyDataRepository.AddAsync(dailyData, ct);
            count++;
        }
        return count;
    }

    private async Task<Fund?> EnsureFundExistsAsync(FundType fundType, FonGnlBlgResult info, CancellationToken ct)
    {
        var fund = await fundRepository.FirstOrDefaultAsync(f => f.Code == info.FonKodu, ct);
        if (fund is not null) return fund;

        fund = new Fund
        {
            Id = Guid.NewGuid(),
            Code = info.FonKodu,
            Title = info.FonUnvan,
            FundTypeId = fundType.Id
        };
        await fundRepository.AddAsync(fund, ct);
        await unitOfWork.SaveChangesAsync(ct);
        return fund;
    }

    private static FundDailyData BuildDailyData(Guid fundId, FonGnlBlgResult info, DateTime date,
        Dictionary<(string FonKodu, string Tarih), DagilimResult> distributionMap)
    {
        var dailyData = new FundDailyData
        {
            Id = Guid.NewGuid(),
            FundId = fundId,
            Date = date,
            Price = info.Fiyat,
            ShareCount = info.TedPaySayisi,
            InvestorCount = info.KisiSayisi,
            PortfolioSize = info.PortfoyBuyukluk,
            ExchangeBulletinPrice = info.BorsaBultenFiyat
        };

        var key = (info.FonKodu, info.Tarih);
        if (distributionMap.TryGetValue(key, out var dist))
            dist.Adapt(dailyData);

        return dailyData;
    }

    private static bool HasMorePages(FonGnlBlgResponse response, int currentPage) =>
        response.ToplamSayfa.HasValue && currentPage < response.ToplamSayfa.Value;
}
