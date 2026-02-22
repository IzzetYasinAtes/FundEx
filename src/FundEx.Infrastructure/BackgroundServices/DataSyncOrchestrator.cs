namespace FundEx.Infrastructure.BackgroundServices;
using FundEx.Application.Common.Interfaces;
using FundEx.Application.Common.Models.Tefas;
using FundEx.Domain.Entities;
using FundEx.Infrastructure.ExternalServices.Tefas;
using Mapster;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

public class DataSyncOrchestrator
{
    private readonly IServiceProvider _serviceProvider;
    private readonly TefasApiSettings _settings;
    private readonly ILogger<DataSyncOrchestrator> _logger;

    private static readonly string[] FundTypeCodes = ["YAT", "EMK", "BYF", "GYF", "GSYF"];

    public DataSyncOrchestrator(IServiceProvider serviceProvider, IOptions<TefasApiSettings> settings, ILogger<DataSyncOrchestrator> logger)
    {
        _serviceProvider = serviceProvider;
        _settings = settings.Value;
        _logger = logger;
    }

    public async Task SyncMetadataAsync(CancellationToken ct)
    {
        using var scope = _serviceProvider.CreateScope();
        var tefas = scope.ServiceProvider.GetRequiredService<ITefasApiService>();
        var fundTypeRepo = scope.ServiceProvider.GetRequiredService<IRepository<FundType>>();
        var founderRepo = scope.ServiceProvider.GetRequiredService<IRepository<Founder>>();
        var categoryRepo = scope.ServiceProvider.GetRequiredService<IRepository<FundCategory>>();
        var titleRepo = scope.ServiceProvider.GetRequiredService<IRepository<FundTitle>>();
        var uow = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

        foreach (var typeCode in FundTypeCodes)
        {
            _logger.LogInformation("Syncing metadata for {FundType}", typeCode);
            var fundType = await fundTypeRepo.FirstOrDefaultAsync(ft => ft.Code == typeCode, ct);
            if (fundType is null) continue;

            var founders = await tefas.GetFoundersAsync(typeCode, ct: ct);
            foreach (var f in founders)
            {
                var exists = await founderRepo.AnyAsync(x => x.Code == f.KurucuKodu && x.FundTypeCode == typeCode, ct);
                if (!exists)
                    await founderRepo.AddAsync(new Founder
                    {
                        Id = Guid.NewGuid(),
                        Code = f.KurucuKodu,
                        Title = f.KurucuUnvan,
                        FundTypeCode = typeCode,
                        FundTypeId = fundType.Id
                    }, ct);
            }

            if (typeCode == "YAT")
            {
                var types = await tefas.GetFundTypesAsync(ct: ct);
                foreach (var t in types)
                {
                    var code = t.SfonTuru.ToString();
                    var exists = await categoryRepo.AnyAsync(x => x.Code == code && x.FundTypeId == fundType.Id, ct);
                    if (!exists)
                        await categoryRepo.AddAsync(new FundCategory
                        {
                            Id = Guid.NewGuid(), Code = code, Name = t.SfonTurAciklama, FundTypeId = fundType.Id
                        }, ct);
                }
            }

            if (typeCode is "EMK" or "BYF")
            {
                var details = await tefas.GetFundDetailsAsync(typeCode, ct: ct);
                foreach (var d in details)
                {
                    var exists = await categoryRepo.AnyAsync(x => x.Code == d.FonTurKod && x.FundTypeId == fundType.Id, ct);
                    if (!exists)
                        await categoryRepo.AddAsync(new FundCategory
                        {
                            Id = Guid.NewGuid(), Code = d.FonTurKod, Name = d.FonTurAciklama, FundTypeId = fundType.Id
                        }, ct);
                }
            }

            if (typeCode == "EMK")
            {
                var groups = await tefas.GetFundGroupsAsync(ct: ct);
                foreach (var g in groups)
                {
                    var exists = await titleRepo.AnyAsync(x => x.GroupCode == g.FonGrubu && x.FundTypeId == fundType.Id, ct);
                    if (!exists)
                        await titleRepo.AddAsync(new FundTitle
                        {
                            Id = Guid.NewGuid(), GroupCode = g.FonGrubu, Name = g.Fongrupaciklama, FundTypeId = fundType.Id
                        }, ct);
                }
            }
            else
            {
                var titles = await tefas.GetFundTitlesAsync(typeCode, ct: ct);
                foreach (var t in titles)
                {
                    var exists = await titleRepo.AnyAsync(x => x.Name == t.Tanim && x.FundTypeId == fundType.Id, ct);
                    if (!exists)
                        await titleRepo.AddAsync(new FundTitle
                        {
                            Id = Guid.NewGuid(), Name = t.Tanim, FundTypeId = fundType.Id
                        }, ct);
                }
            }

            await uow.SaveChangesAsync(ct);
        }
    }

    public async Task SyncDailyDataAsync(DateTime startDate, DateTime endDate, CancellationToken ct)
    {
        _logger.LogInformation("Syncing daily data from {Start} to {End}", startDate, endDate);
        var currentStart = startDate;

        while (currentStart < endDate)
        {
            var currentEnd = currentStart.AddMonths(1) > endDate ? endDate : currentStart.AddMonths(1);
            var startStr = currentStart.ToString("yyyyMMdd");
            var endStr = currentEnd.ToString("yyyyMMdd");

            foreach (var typeCode in FundTypeCodes)
            {
                await SyncFundTypeDataAsync(typeCode, startStr, endStr, ct);
            }

            currentStart = currentEnd.AddDays(1);
        }
    }

    private async Task SyncFundTypeDataAsync(string typeCode, string startDate, string endDate, CancellationToken ct)
    {
        using var scope = _serviceProvider.CreateScope();
        var tefas = scope.ServiceProvider.GetRequiredService<ITefasApiService>();
        var fundRepo = scope.ServiceProvider.GetRequiredService<IRepository<Fund>>();
        var fundTypeRepo = scope.ServiceProvider.GetRequiredService<IRepository<FundType>>();
        var dailyRepo = scope.ServiceProvider.GetRequiredService<IRepository<FundDailyData>>();
        var uow = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

        var fundType = await fundTypeRepo.FirstOrDefaultAsync(ft => ft.Code == typeCode, ct);
        if (fundType is null) return;

        var pageSize = _settings.PageSize;
        var page = 1;
        var hasMore = true;

        while (hasMore)
        {
            var request = new FonGnlBlgRequest
            {
                FonTipi = typeCode, BasTarih = startDate, BitTarih = endDate,
                BasSira = (page - 1) * pageSize + 1, BitSira = page * pageSize
            };

            var generalInfo = await tefas.GetFundGeneralInfoAsync(request, ct);
            if (generalInfo.ResultList.Count == 0) break;

            var distRequest = new DagilimRequest
            {
                FonTipi = typeCode, BasTarih = startDate, BitTarih = endDate,
                BasSira = request.BasSira, BitSira = request.BitSira
            };
            var distribution = await tefas.GetFundDistributionAsync(distRequest, ct);
            var distMap = distribution.ResultList
                .GroupBy(d => new { d.FonKodu, d.Tarih })
                .ToDictionary(g => g.Key, g => g.First());

            foreach (var info in generalInfo.ResultList)
            {
                var fund = await fundRepo.FirstOrDefaultAsync(f => f.Code == info.FonKodu, ct);
                if (fund is null)
                {
                    fund = new Fund
                    {
                        Id = Guid.NewGuid(), Code = info.FonKodu, Title = info.FonUnvan, FundTypeId = fundType.Id
                    };
                    await fundRepo.AddAsync(fund, ct);
                    await uow.SaveChangesAsync(ct);
                }

                if (!DateTime.TryParse(info.Tarih, out var date)) continue;

                var exists = await dailyRepo.AnyAsync(d => d.FundId == fund.Id && d.Date == date, ct);
                if (exists) continue;

                var dailyData = new FundDailyData
                {
                    Id = Guid.NewGuid(),
                    FundId = fund.Id,
                    Date = date,
                    Price = info.Fiyat,
                    ShareCount = info.TedPaySayisi,
                    InvestorCount = info.KisiSayisi,
                    PortfolioSize = info.PortfoyBuyukluk,
                    ExchangeBulletinPrice = info.BorsaBultenFiyat
                };

                var key = new { FonKodu = info.FonKodu, Tarih = info.Tarih };
                if (distMap.TryGetValue(key, out var dist))
                    dist.Adapt(dailyData);

                await dailyRepo.AddAsync(dailyData, ct);
            }

            await uow.SaveChangesAsync(ct);
            _logger.LogDebug("Synced page {Page} for {Type} ({Start}-{End})", page, typeCode, startDate, endDate);

            hasMore = generalInfo.ToplamSayfa.HasValue && page < generalInfo.ToplamSayfa.Value;
            page++;
        }
    }

    public async Task RunFullSyncAsync(CancellationToken ct)
    {
        _logger.LogInformation("Starting full data sync...");
        await SyncMetadataAsync(ct);

        var endDate = DateTime.UtcNow.Date;
        var startDate = endDate.AddYears(-_settings.BackfillYears);
        await SyncDailyDataAsync(startDate, endDate, ct);
        _logger.LogInformation("Full data sync completed");
    }

    public async Task RunDailySyncAsync(CancellationToken ct)
    {
        _logger.LogInformation("Starting daily sync...");
        await SyncMetadataAsync(ct);

        var today = DateTime.UtcNow.Date;
        await SyncDailyDataAsync(today.AddDays(-1), today, ct);
        _logger.LogInformation("Daily sync completed");
    }
}
