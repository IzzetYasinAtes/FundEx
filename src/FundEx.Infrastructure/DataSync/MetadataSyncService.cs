namespace FundEx.Infrastructure.DataSync;

using FundEx.Application.Common.Constants;
using FundEx.Application.Common.Interfaces;
using FundEx.Application.Common.Models.Tefas;
using FundEx.Domain.Entities;
using Microsoft.Extensions.Logging;

public sealed class MetadataSyncService : IMetadataSyncService
{
    private readonly ITefasApiService _tefasApi;
    private readonly IRepository<FundType> _fundTypeRepository;
    private readonly IRepository<Founder> _founderRepository;
    private readonly IRepository<FundCategory> _categoryRepository;
    private readonly IRepository<FundTitle> _titleRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<MetadataSyncService> _logger;

    public MetadataSyncService(
        ITefasApiService tefasApi,
        IRepository<FundType> fundTypeRepository,
        IRepository<Founder> founderRepository,
        IRepository<FundCategory> categoryRepository,
        IRepository<FundTitle> titleRepository,
        IUnitOfWork unitOfWork,
        ILogger<MetadataSyncService> logger)
    {
        _tefasApi = tefasApi;
        _fundTypeRepository = fundTypeRepository;
        _founderRepository = founderRepository;
        _categoryRepository = categoryRepository;
        _titleRepository = titleRepository;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task SyncAsync(string fundTypeCode, CancellationToken cancellationToken = default)
    {
        var fundType = await _fundTypeRepository.FirstOrDefaultAsync(ft => ft.Code == fundTypeCode, cancellationToken);
        if (fundType is null)
        {
            return;
        }

        _logger.LogInformation("Syncing metadata for {FundType}", fundTypeCode);

        await SyncFoundersAsync(fundType, fundTypeCode, cancellationToken);
        await SyncCategoriesAsync(fundType, fundTypeCode, cancellationToken);
        await SyncTitlesAsync(fundType, fundTypeCode, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }

    private async Task SyncFoundersAsync(FundType fundType, string fundTypeCode, CancellationToken ct)
    {
        var items = await _tefasApi.GetFoundersAsync(fundTypeCode, ct: ct);
        foreach (var item in items)
        {
            if (await _founderRepository.AnyAsync(x => x.Code == item.KurucuKodu && x.FundTypeCode == fundTypeCode, ct))
            {
                continue;
            }
            await _founderRepository.AddAsync(new Founder
            {
                Id = Guid.NewGuid(),
                Code = item.KurucuKodu,
                Title = item.KurucuUnvan,
                FundTypeCode = fundTypeCode,
                FundTypeId = fundType.Id
            }, ct);
        }
        _logger.LogInformation("Synced {Count} founders for {FundType}", items.Count, fundTypeCode);
    }

    private async Task SyncCategoriesAsync(FundType fundType, string fundTypeCode, CancellationToken ct)
    {
        if (fundTypeCode == FundTypeCodeConstants.YAT)
        {
            await SyncCategoriesFromFundTypesAsync(fundType, ct);
        }
        else if (fundTypeCode is FundTypeCodeConstants.EMK or FundTypeCodeConstants.BYF)
        {
            await SyncCategoriesFromFundDetailsAsync(fundType, fundTypeCode, ct);
        }
    }

    private async Task SyncCategoriesFromFundTypesAsync(FundType fundType, CancellationToken ct)
    {
        var items = await _tefasApi.GetFundTypesAsync(ct: ct);
        foreach (var item in items)
        {
            var code = item.SfonTuru.ToString();
            if (await _categoryRepository.AnyAsync(x => x.Code == code && x.FundTypeId == fundType.Id, ct))
            {
                continue;
            }
            await _categoryRepository.AddAsync(new FundCategory
            {
                Id = Guid.NewGuid(), Code = code, Name = item.SfonTurAciklama, FundTypeId = fundType.Id
            }, ct);
        }
    }

    private async Task SyncCategoriesFromFundDetailsAsync(FundType fundType, string fundTypeCode, CancellationToken ct)
    {
        var items = await _tefasApi.GetFundDetailsAsync(fundTypeCode, ct: ct);
        foreach (var item in items)
        {
            if (await _categoryRepository.AnyAsync(x => x.Code == item.FonTurKod && x.FundTypeId == fundType.Id, ct))
            {
                continue;
            }
            await _categoryRepository.AddAsync(new FundCategory
            {
                Id = Guid.NewGuid(), Code = item.FonTurKod, Name = item.FonTurAciklama, FundTypeId = fundType.Id
            }, ct);
        }
    }

    private async Task SyncTitlesAsync(FundType fundType, string fundTypeCode, CancellationToken ct)
    {
        if (fundTypeCode == FundTypeCodeConstants.EMK)
        {
            await SyncTitlesFromFundGroupsAsync(fundType, ct);
        }
        else
        {
            await SyncTitlesFromFundUnvanAsync(fundType, fundTypeCode, ct);
        }
    }

    private async Task SyncTitlesFromFundGroupsAsync(FundType fundType, CancellationToken ct)
    {
        var items = await _tefasApi.GetFundGroupsAsync(ct: ct);
        foreach (var item in items)
        {
            if (await _titleRepository.AnyAsync(x => x.GroupCode == item.FonGrubu && x.FundTypeId == fundType.Id, ct))
            {
                continue;
            }
            await _titleRepository.AddAsync(new FundTitle
            {
                Id = Guid.NewGuid(), GroupCode = item.FonGrubu, Name = item.Fongrupaciklama, FundTypeId = fundType.Id
            }, ct);
        }
    }

    private async Task SyncTitlesFromFundUnvanAsync(FundType fundType, string fundTypeCode, CancellationToken ct)
    {
        var items = await _tefasApi.GetFundTitlesAsync(fundTypeCode, ct: ct);
        foreach (var item in items)
        {
            if (await _titleRepository.AnyAsync(x => x.Name == item.Tanim && x.FundTypeId == fundType.Id, ct))
            {
                continue;
            }
            await _titleRepository.AddAsync(new FundTitle
            {
                Id = Guid.NewGuid(), Name = item.Tanim, FundTypeId = fundType.Id
            }, ct);
        }
    }
}
