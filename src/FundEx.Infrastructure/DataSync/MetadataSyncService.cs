namespace FundEx.Infrastructure.DataSync;

using FundEx.Application.Common.Interfaces;
using FundEx.Application.Common.Models.Tefas;
using FundEx.Domain.Entities;
using Microsoft.Extensions.Logging;

public sealed class MetadataSyncService(
    ITefasApiService tefasApi,
    IRepository<FundType> fundTypeRepository,
    IRepository<Founder> founderRepository,
    IRepository<FundCategory> categoryRepository,
    IRepository<FundTitle> titleRepository,
    IUnitOfWork unitOfWork,
    ILogger<MetadataSyncService> logger) : IMetadataSyncService
{
    public async Task SyncAsync(string fundTypeCode, CancellationToken cancellationToken = default)
    {
        var fundType = await fundTypeRepository.FirstOrDefaultAsync(ft => ft.Code == fundTypeCode, cancellationToken);
        if (fundType is null) return;

        logger.LogInformation("Syncing metadata for {FundType}", fundTypeCode);

        await SyncFoundersAsync(fundType, fundTypeCode, cancellationToken);
        await SyncCategoriesAsync(fundType, fundTypeCode, cancellationToken);
        await SyncTitlesAsync(fundType, fundTypeCode, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
    }

    private async Task SyncFoundersAsync(FundType fundType, string fundTypeCode, CancellationToken ct)
    {
        var founders = await tefasApi.GetFoundersAsync(fundTypeCode, ct: ct);
        if (founders.Count == 0) return;

        foreach (var item in founders)
        {
            var exists = await founderRepository.AnyAsync(x => x.Code == item.KurucuKodu && x.FundTypeCode == fundTypeCode, ct);
            if (!exists)
                await founderRepository.AddAsync(CreateFounder(item, fundType, fundTypeCode), ct);
        }

        logger.LogInformation("Synced {Count} founders for {FundType}", founders.Count, fundTypeCode);
    }

    private async Task SyncCategoriesAsync(FundType fundType, string fundTypeCode, CancellationToken ct)
    {
        if (fundTypeCode == FundTypeCodes.YAT)
            await SyncCategoriesFromFundTypesAsync(fundType, ct);
        else if (fundTypeCode is FundTypeCodes.EMK or FundTypeCodes.BYF)
            await SyncCategoriesFromFundDetailsAsync(fundType, fundTypeCode, ct);
    }

    private async Task SyncCategoriesFromFundTypesAsync(FundType fundType, CancellationToken ct)
    {
        var types = await tefasApi.GetFundTypesAsync(ct: ct);
        foreach (var item in types)
        {
            var code = item.SfonTuru.ToString();
            if (await categoryRepository.AnyAsync(x => x.Code == code && x.FundTypeId == fundType.Id, ct)) continue;
            await categoryRepository.AddAsync(new FundCategory
            {
                Id = Guid.NewGuid(), Code = code, Name = item.SfonTurAciklama, FundTypeId = fundType.Id
            }, ct);
        }
    }

    private async Task SyncCategoriesFromFundDetailsAsync(FundType fundType, string fundTypeCode, CancellationToken ct)
    {
        var details = await tefasApi.GetFundDetailsAsync(fundTypeCode, ct: ct);
        foreach (var item in details)
        {
            if (await categoryRepository.AnyAsync(x => x.Code == item.FonTurKod && x.FundTypeId == fundType.Id, ct)) continue;
            await categoryRepository.AddAsync(new FundCategory
            {
                Id = Guid.NewGuid(), Code = item.FonTurKod, Name = item.FonTurAciklama, FundTypeId = fundType.Id
            }, ct);
        }
    }

    private async Task SyncTitlesAsync(FundType fundType, string fundTypeCode, CancellationToken ct)
    {
        if (fundTypeCode == FundTypeCodes.EMK)
            await SyncTitlesFromFundGroupsAsync(fundType, ct);
        else
            await SyncTitlesFromFundUnvanAsync(fundType, fundTypeCode, ct);
    }

    private async Task SyncTitlesFromFundGroupsAsync(FundType fundType, CancellationToken ct)
    {
        var groups = await tefasApi.GetFundGroupsAsync(ct: ct);
        foreach (var item in groups)
        {
            if (await titleRepository.AnyAsync(x => x.GroupCode == item.FonGrubu && x.FundTypeId == fundType.Id, ct)) continue;
            await titleRepository.AddAsync(new FundTitle
            {
                Id = Guid.NewGuid(), GroupCode = item.FonGrubu, Name = item.Fongrupaciklama, FundTypeId = fundType.Id
            }, ct);
        }
    }

    private async Task SyncTitlesFromFundUnvanAsync(FundType fundType, string fundTypeCode, CancellationToken ct)
    {
        var titles = await tefasApi.GetFundTitlesAsync(fundTypeCode, ct: ct);
        foreach (var item in titles)
        {
            if (await titleRepository.AnyAsync(x => x.Name == item.Tanim && x.FundTypeId == fundType.Id, ct)) continue;
            await titleRepository.AddAsync(new FundTitle
            {
                Id = Guid.NewGuid(), Name = item.Tanim, FundTypeId = fundType.Id
            }, ct);
        }
    }

    private static Founder CreateFounder(FonKurucuResult item, FundType fundType, string fundTypeCode) =>
        new()
        {
            Id = Guid.NewGuid(),
            Code = item.KurucuKodu,
            Title = item.KurucuUnvan,
            FundTypeCode = fundTypeCode,
            FundTypeId = fundType.Id
        };

    private static class FundTypeCodes
    {
        public const string YAT = "YAT";
        public const string EMK = "EMK";
        public const string BYF = "BYF";
    }
}
