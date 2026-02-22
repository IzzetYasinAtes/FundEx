using System.ComponentModel;
using FundEx.Domain.Common;

namespace FundEx.Domain.Entities;

[Description("""
    Fund Daily Data / Fon Gunluk Verisi

    EN: Combined daily data from fonGnlBlgSiraliGetir (general info) and dagilimSiraliGetirT (distribution) APIs.
        Each record represents a single fund's data for a specific date.
    TR: fonGnlBlgSiraliGetir (genel bilgi) ve dagilimSiraliGetirT (dagilim) API'lerinden birlestirilen gunluk veri.
        Her kayit, belirli bir tarih icin tek bir fonun verisini temsil eder.
    """)]
public class FundDailyData : Entity<Guid>
{
    public Guid FundId { get; set; }
    public virtual Fund Fund { get; set; } = null!;

    [Description("""
        Date / Tarih

        EN: The date this daily data record belongs to.
        TR: Bu gunluk veri kaydinin ait oldugu tarih.
        """)]
    public DateTime Date { get; set; }

    #region General Info (fonGnlBlgSiraliGetir)

    [Description("""
        Price / Fiyat

        EN: Unit share price of the fund on this date.
        TR: Fonun bu tarihteki birim pay fiyati.
        """)]
    public decimal Price { get; set; }

    [Description("""
        Share Count / Tedavuldeki Pay Sayisi

        EN: Total number of fund shares in circulation.
        TR: Tedavuldeki toplam fon pay sayisi.
        """)]
    public long ShareCount { get; set; }

    [Description("""
        Investor Count / Yatirimci Sayisi

        EN: Number of individual investors holding the fund.
        TR: Fonu elinde bulunduran bireysel yatirimci sayisi.
        """)]
    public int InvestorCount { get; set; }

    [Description("""
        Portfolio Size / Portfoy Buyuklugu

        EN: Total portfolio value of the fund in TRY.
        TR: Fonun TL cinsinden toplam portfoy degeri.
        """)]
    public decimal PortfolioSize { get; set; }

    [Description("""
        Exchange Bulletin Price / Borsa Bulten Fiyati

        EN: Price published in the stock exchange bulletin. Null for non-exchange traded funds.
        TR: Borsa bulteninde yayinlanan fiyat. Borsada islem gormeyen fonlar icin null.
        """)]
    public decimal? ExchangeBulletinPrice { get; set; }

    #endregion

    #region Distribution Percentages (dagilimSiraliGetirT)

    [Description("""
        Stock / Hisse Senedi (%)

        EN: Percentage of fund portfolio allocated to equity stocks.
        TR: Fon portfoyunun hisse senedine ayrilan yuzdesi.
        """)]
    public decimal? Stock { get; set; }

    [Description("""
        Government Bond / Devlet Tahvili (%)

        EN: Percentage allocated to government bonds.
        TR: Devlet tahvillerine ayrilan yuzde.
        """)]
    public decimal? GovernmentBond { get; set; }

    [Description("""
        FX Denominated Government Debt / Doviz Cinsi Kamu Ic Borclanma Araclari (%)

        EN: Percentage allocated to foreign currency denominated government domestic debt instruments.
        TR: Doviz cinsi kamu ic borclanma araclarina ayrilan yuzde.
        """)]
    public decimal? FxDenominatedGovernmentDebt { get; set; }

    [Description("""
        Treasury Bill / Hazine Bonosu (%)

        EN: Percentage allocated to treasury bills.
        TR: Hazine bonolarina ayrilan yuzde.
        """)]
    public decimal? TreasuryBill { get; set; }

    [Description("""
        Commercial Paper / Finansman Bonosu (%)

        EN: Percentage allocated to commercial papers (corporate short-term debt).
        TR: Finansman bonolarina (kurumsal kisa vadeli borc) ayrilan yuzde.
        """)]
    public decimal? CommercialPaper { get; set; }

    [Description("""
        Corporate Bond / Ozel Sektor Tahvili (%)

        EN: Percentage allocated to corporate bonds.
        TR: Ozel sektor tahvillerine ayrilan yuzde.
        """)]
    public decimal? CorporateBond { get; set; }

    [Description("""
        Asset Backed Securities / Varliga Dayali Menkul Kiymetler (%)

        EN: Percentage allocated to asset-backed securities.
        TR: Varliga dayali menkul kiymetlere ayrilan yuzde.
        """)]
    public decimal? AssetBackedSecurities { get; set; }

    [Description("""
        Government External Debt / Kamu Dis Borclanma Araclari (%)

        EN: Percentage allocated to government external debt instruments.
        TR: Kamu dis borclanma araclarina ayrilan yuzde.
        """)]
    public decimal? GovernmentExternalDebt { get; set; }

    [Description("""
        Corporate External Debt / Ozel Sektor Dis Borclanma Araclari (%)

        EN: Percentage allocated to corporate external debt instruments.
        TR: Ozel sektor dis borclanma araclarina ayrilan yuzde.
        """)]
    public decimal? CorporateExternalDebt { get; set; }

    [Description("""
        Government Lease Certificates / Kamu Kira Sertifikalari (%)

        EN: Percentage allocated to government lease certificates (sukuk).
        TR: Kamu kira sertifikalarina (sukuk) ayrilan yuzde.
        """)]
    public decimal? GovernmentLeaseCertificates { get; set; }

    [Description("""
        Government Lease Certificates TL / Kamu Kira Sertifikalari TL (%)

        EN: Percentage allocated to TRY denominated government lease certificates.
        TR: TL cinsi kamu kira sertifikalarina ayrilan yuzde.
        """)]
    public decimal? GovernmentLeaseCertificatesTl { get; set; }

    [Description("""
        Government Lease Certificates FX / Kamu Kira Sertifikalari Doviz (%)

        EN: Percentage allocated to foreign currency denominated government lease certificates.
        TR: Doviz cinsi kamu kira sertifikalarina ayrilan yuzde.
        """)]
    public decimal? GovernmentLeaseCertificatesFx { get; set; }

    [Description("""
        Lease Certificates Non-Investment / Kira Sertifikalari Yatirim Disi (%)

        EN: Percentage allocated to non-investment grade lease certificates.
        TR: Yatirim disi kira sertifikalarina ayrilan yuzde.
        """)]
    public decimal? LeaseCertificatesNonInvestment { get; set; }

    [Description("""
        Corporate Lease Certificates / Ozel Sektor Kira Sertifikalari (%)

        EN: Percentage allocated to corporate lease certificates.
        TR: Ozel sektor kira sertifikalarina ayrilan yuzde.
        """)]
    public decimal? CorporateLeaseCertificates { get; set; }

    [Description("""
        Reverse Repo / Ters Repo (%)

        EN: Percentage allocated to reverse repurchase agreements.
        TR: Ters repo islemlerine ayrilan yuzde.
        """)]
    public decimal? ReverseRepo { get; set; }

    [Description("""
        Repo / Repo (%)

        EN: Percentage allocated to repurchase agreements.
        TR: Repo islemlerine ayrilan yuzde.
        """)]
    public decimal? Repo { get; set; }

    [Description("""
        BIST Committed Trading Sale / BIST Taahhutu Islem Pazari Satim (%)

        EN: Percentage in BIST committed trading platform sale transactions.
        TR: BIST taahhutu islem pazari satim islemlerine ayrilan yuzde.
        """)]
    public decimal? BistCommittedTradingSale { get; set; }

    [Description("""
        BIST Committed Trading Purchase / BIST Taahhutu Islem Pazari Alim (%)

        EN: Percentage in BIST committed trading platform purchase transactions.
        TR: BIST taahhutu islem pazari alim islemlerine ayrilan yuzde.
        """)]
    public decimal? BistCommittedTradingPurchase { get; set; }

    [Description("""
        Takasbank Money Market / Takasbank Para Piyasasi (%)

        EN: Percentage allocated to Takasbank money market operations.
        TR: Takasbank para piyasasi islemlerine ayrilan yuzde.
        """)]
    public decimal? TakasbankMoneyMarket { get; set; }

    [Description("""
        Borsa Istanbul Money Market / Borsa Istanbul Para Piyasasi (%)

        EN: Percentage allocated to Borsa Istanbul money market operations.
        TR: Borsa Istanbul para piyasasi islemlerine ayrilan yuzde.
        """)]
    public decimal? BorsaIstanbulMoneyMarket { get; set; }

    [Description("""
        Deposit FX / Mevduat Doviz (%)

        EN: Percentage allocated to foreign currency deposits.
        TR: Doviz mevduatina ayrilan yuzde.
        """)]
    public decimal? DepositFx { get; set; }

    [Description("""
        Deposit TL / Mevduat TL (%)

        EN: Percentage allocated to Turkish Lira deposits.
        TR: TL mevduatina ayrilan yuzde.
        """)]
    public decimal? DepositTl { get; set; }

    [Description("""
        Deposit / Mevduat (%)

        EN: Percentage allocated to general deposits.
        TR: Genel mevduata ayrilan yuzde.
        """)]
    public decimal? Deposit { get; set; }

    [Description("""
        Deposit Gold / Mevduat Altin (%)

        EN: Percentage allocated to gold deposits.
        TR: Altin mevduatina ayrilan yuzde.
        """)]
    public decimal? DepositGold { get; set; }

    [Description("""
        Participation Account FX / Katilma Hesabi Doviz (%)

        EN: Percentage allocated to foreign currency participation accounts (Islamic banking).
        TR: Doviz katilma hesaplarina (katilim bankaciligi) ayrilan yuzde.
        """)]
    public decimal? ParticipationAccountFx { get; set; }

    [Description("""
        Participation Account TL / Katilma Hesabi TL (%)

        EN: Percentage allocated to TRY participation accounts.
        TR: TL katilma hesaplarina ayrilan yuzde.
        """)]
    public decimal? ParticipationAccountTl { get; set; }

    [Description("""
        Participation Account / Katilma Hesabi (%)

        EN: Percentage allocated to general participation accounts.
        TR: Genel katilma hesaplarina ayrilan yuzde.
        """)]
    public decimal? ParticipationAccount { get; set; }

    [Description("""
        Participation Account Gold / Katilma Hesabi Altin (%)

        EN: Percentage allocated to gold participation accounts.
        TR: Altin katilma hesaplarina ayrilan yuzde.
        """)]
    public decimal? ParticipationAccountGold { get; set; }

    [Description("""
        Precious Metals / Kiymetli Madenler (%)

        EN: Percentage allocated to precious metals (gold, silver, etc.).
        TR: Kiymetli madenlere (altin, gumus vb.) ayrilan yuzde.
        """)]
    public decimal? PreciousMetals { get; set; }

    [Description("""
        Precious Metals ETF / Kiymetli Madenler BYF (%)

        EN: Percentage allocated to precious metals exchange traded funds.
        TR: Kiymetli madenler borsa yatirim fonlarina ayrilan yuzde.
        """)]
    public decimal? PreciousMetalsEtf { get; set; }

    [Description("""
        Precious Metals Government Debt / Kiymetli Madenler Kamu Borclanma (%)

        EN: Percentage allocated to precious metals backed government debt.
        TR: Kiymetli maden destekli kamu borclanma araclarina ayrilan yuzde.
        """)]
    public decimal? PreciousMetalsGovernmentDebt { get; set; }

    [Description("""
        Precious Metals Lease Certificates / Kiymetli Madenler Kira Sertifikalari (%)

        EN: Percentage allocated to precious metals lease certificates.
        TR: Kiymetli maden kira sertifikalarina ayrilan yuzde.
        """)]
    public decimal? PreciousMetalsLeaseCertificates { get; set; }

    [Description("""
        Foreign ETF / Yabanci Borsa Yatirim Fonlari (%)

        EN: Percentage allocated to foreign exchange traded funds.
        TR: Yabanci borsa yatirim fonlarina ayrilan yuzde.
        """)]
    public decimal? ForeignEtf { get; set; }

    [Description("""
        Foreign Stock / Yabanci Hisse Senedi (%)

        EN: Percentage allocated to foreign equity stocks.
        TR: Yabanci hisse senetlerine ayrilan yuzde.
        """)]
    public decimal? ForeignStock { get; set; }

    [Description("""
        ETF Participation Shares / Borsa Yatirim Fonlari Katilma Paylari (%)

        EN: Percentage allocated to ETF participation shares.
        TR: Borsa yatirim fonlari katilma paylarina ayrilan yuzde.
        """)]
    public decimal? EtfParticipationShares { get; set; }

    [Description("""
        Foreign Partnership Debt / Yabanci Ortaklik Borclanma (%)

        EN: Percentage allocated to foreign partnership debt instruments.
        TR: Yabanci ortaklik borclanma araclarina ayrilan yuzde.
        """)]
    public decimal? ForeignPartnershipDebt { get; set; }

    [Description("""
        Foreign Debt Instruments / Yabanci Borclanma Araclari (%)

        EN: Percentage allocated to foreign debt instruments.
        TR: Yabanci borclanma araclarina ayrilan yuzde.
        """)]
    public decimal? ForeignDebtInstruments { get; set; }

    [Description("""
        Foreign Securities / Yabanci Menkul Kiymetler (%)

        EN: Percentage allocated to foreign securities.
        TR: Yabanci menkul kiymetlere ayrilan yuzde.
        """)]
    public decimal? ForeignSecurities { get; set; }

    [Description("""
        Investment Fund Shares / Yatirim Fonlari Katilma Paylari (%)

        EN: Percentage allocated to other investment fund participation shares.
        TR: Diger yatirim fonlari katilma paylarina ayrilan yuzde.
        """)]
    public decimal? InvestmentFundShares { get; set; }

    [Description("""
        Futures Cash Collateral / Vadeli Islemler Nakit Teminatlari (%)

        EN: Percentage held as cash collateral for futures/derivatives positions.
        TR: Vadeli islem/turev pozisyonlari icin nakit teminat olarak tutulan yuzde.
        """)]
    public decimal? FuturesCashCollateral { get; set; }

    [Description("""
        Other / Diger (%)

        EN: Percentage allocated to other/unclassified asset categories.
        TR: Diger/siniflandirilmamis varlik kategorilerine ayrilan yuzde.
        """)]
    public decimal? Other { get; set; }

    [Description("""
        Exchange Traded Fund / Borsa Yatirim Fonu (%)

        EN: Percentage allocated to domestic exchange traded funds.
        TR: Yurt ici borsa yatirim fonlarina ayrilan yuzde.
        """)]
    public decimal? ExchangeTradedFund { get; set; }

    [Description("""
        Other Partnership / Diger Ortaklik (%)

        EN: Percentage allocated to other partnership investments.
        TR: Diger ortaklik yatirimlarina ayrilan yuzde.
        """)]
    public decimal? OtherPartnership { get; set; }

    [Description("""
        Eurobond / Eurotahvil (%)

        EN: Percentage allocated to eurobonds.
        TR: Eurotahvillere ayrilan yuzde.
        """)]
    public decimal? Eurobond { get; set; }

    [Description("""
        Fund Government Debt / Fonlar Kamu Borclanma (%)

        EN: Percentage allocated to fund-held government debt instruments.
        TR: Fonlardaki kamu borclanma araclarina ayrilan yuzde.
        """)]
    public decimal? FundGovernmentDebt { get; set; }

    [Description("""
        Real Estate Trading / Gayrimenkul Alim Satim (%)

        EN: Percentage allocated to real estate trading activities.
        TR: Gayrimenkul alim satim faaliyetlerine ayrilan yuzde.
        """)]
    public decimal? RealEstateTrading { get; set; }

    [Description("""
        Venture Capital Institution Debt / Girisim Sermayesi Yatirim Kurulusu Borclanma (%)

        EN: Percentage allocated to venture capital institution debt instruments.
        TR: Girisim sermayesi yatirim kurulusu borclanma araclarina ayrilan yuzde.
        """)]
    public decimal? VentureCapitalInstitutionDebt { get; set; }

    [Description("""
        Venture Capital Investment / Girisim Sermayesi Yatirimi (%)

        EN: Percentage allocated to direct venture capital investments.
        TR: Dogrudan girisim sermayesi yatirimlarina ayrilan yuzde.
        """)]
    public decimal? VentureCapitalInvestment { get; set; }

    [Description("""
        Real Estate Institution Debt / Gayrimenkul Yatirim Kurulusu Borclanma (%)

        EN: Percentage allocated to real estate investment institution debt instruments.
        TR: Gayrimenkul yatirim kurulusu borclanma araclarina ayrilan yuzde.
        """)]
    public decimal? RealEstateInstitutionDebt { get; set; }

    [Description("""
        Real Estate Investment / Gayrimenkul Yatirimi (%)

        EN: Percentage allocated to direct real estate investments.
        TR: Dogrudan gayrimenkul yatirimlarina ayrilan yuzde.
        """)]
    public decimal? RealEstateInvestment { get; set; }

    [Description("""
        Government Debt Instruments / Kamu Borclanma Araclari (%)

        EN: Percentage allocated to general government debt instruments.
        TR: Genel kamu borclanma araclarina ayrilan yuzde.
        """)]
    public decimal? GovernmentDebtInstruments { get; set; }

    [Description("""
        Leasing Transaction Debt / Kiralama Islemi Borclanma (%)

        EN: Percentage allocated to leasing transaction debt.
        TR: Kiralama islemi borclanmasina ayrilan yuzde.
        """)]
    public decimal? LeasingTransactionDebt { get; set; }

    [Description("""
        Private Sector Non-Investment / Ozel Kesim Yatirim Disi (%)

        EN: Percentage allocated to private sector non-investment grade instruments.
        TR: Ozel kesim yatirim disi araclarina ayrilan yuzde.
        """)]
    public decimal? PrivateSectorNonInvestment { get; set; }

    [Description("""
        Bond / Tahvil (%)

        EN: Percentage allocated to general bonds.
        TR: Genel tahvillere ayrilan yuzde.
        """)]
    public decimal? Bond { get; set; }

    #endregion
}
