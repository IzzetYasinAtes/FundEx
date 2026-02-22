namespace FundEx.Persistence.EntityConfigurations;
using FundEx.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class FundDailyDataConfiguration : IEntityTypeConfiguration<FundDailyData>
{
    public void Configure(EntityTypeBuilder<FundDailyData> builder)
    {
        builder.ToTable("FundDailyData");
        builder.HasKey(x => x.Id);
        builder.HasIndex(x => new { x.FundId, x.Date }).IsUnique();
        builder.HasIndex(x => x.Date);

        builder.Property(x => x.Price).HasPrecision(18, 6);
        builder.Property(x => x.PortfolioSize).HasPrecision(18, 2);
        builder.Property(x => x.ExchangeBulletinPrice).HasPrecision(18, 6);

        var distributionProperties = new[]
        {
            nameof(FundDailyData.Stock), nameof(FundDailyData.GovernmentBond),
            nameof(FundDailyData.FxDenominatedGovernmentDebt), nameof(FundDailyData.TreasuryBill),
            nameof(FundDailyData.CommercialPaper), nameof(FundDailyData.CorporateBond),
            nameof(FundDailyData.AssetBackedSecurities), nameof(FundDailyData.GovernmentExternalDebt),
            nameof(FundDailyData.CorporateExternalDebt), nameof(FundDailyData.GovernmentLeaseCertificates),
            nameof(FundDailyData.GovernmentLeaseCertificatesTl), nameof(FundDailyData.GovernmentLeaseCertificatesFx),
            nameof(FundDailyData.LeaseCertificatesNonInvestment), nameof(FundDailyData.CorporateLeaseCertificates),
            nameof(FundDailyData.ReverseRepo), nameof(FundDailyData.Repo),
            nameof(FundDailyData.BistCommittedTradingSale), nameof(FundDailyData.BistCommittedTradingPurchase),
            nameof(FundDailyData.TakasbankMoneyMarket), nameof(FundDailyData.BorsaIstanbulMoneyMarket),
            nameof(FundDailyData.DepositFx), nameof(FundDailyData.DepositTl),
            nameof(FundDailyData.Deposit), nameof(FundDailyData.DepositGold),
            nameof(FundDailyData.ParticipationAccountFx), nameof(FundDailyData.ParticipationAccountTl),
            nameof(FundDailyData.ParticipationAccount), nameof(FundDailyData.ParticipationAccountGold),
            nameof(FundDailyData.PreciousMetals), nameof(FundDailyData.PreciousMetalsEtf),
            nameof(FundDailyData.PreciousMetalsGovernmentDebt), nameof(FundDailyData.PreciousMetalsLeaseCertificates),
            nameof(FundDailyData.ForeignEtf), nameof(FundDailyData.ForeignStock),
            nameof(FundDailyData.EtfParticipationShares), nameof(FundDailyData.ForeignPartnershipDebt),
            nameof(FundDailyData.ForeignDebtInstruments), nameof(FundDailyData.ForeignSecurities),
            nameof(FundDailyData.InvestmentFundShares), nameof(FundDailyData.FuturesCashCollateral),
            nameof(FundDailyData.Other), nameof(FundDailyData.ExchangeTradedFund),
            nameof(FundDailyData.OtherPartnership), nameof(FundDailyData.Eurobond),
            nameof(FundDailyData.FundGovernmentDebt), nameof(FundDailyData.RealEstateTrading),
            nameof(FundDailyData.VentureCapitalInstitutionDebt), nameof(FundDailyData.VentureCapitalInvestment),
            nameof(FundDailyData.RealEstateInstitutionDebt), nameof(FundDailyData.RealEstateInvestment),
            nameof(FundDailyData.GovernmentDebtInstruments), nameof(FundDailyData.LeasingTransactionDebt),
            nameof(FundDailyData.PrivateSectorNonInvestment), nameof(FundDailyData.Bond)
        };

        foreach (var prop in distributionProperties)
            builder.Property(prop).HasPrecision(8, 2);

        builder.HasOne(x => x.Fund).WithMany(x => x.DailyData).HasForeignKey(x => x.FundId);
    }
}
