using System.ComponentModel;

namespace FundEx.Domain.Enums;

public enum FundTypeCode
{
    [Description("""
        Securities Investment Fund / Menkul Kiymet Yatirim Fonu

        EN: Mutual funds that invest in securities such as stocks, bonds, and other financial instruments.
        TR: Hisse senedi, tahvil ve diger finansal enstrumanlara yatirim yapan yatirim fonlari.
        """)]
    YAT = 1,

    [Description("""
        Pension Fund / Emeklilik Fonu

        EN: Funds managed under the individual pension system for retirement savings.
        TR: Bireysel emeklilik sistemi kapsaminda emeklilik birikimleri icin yonetilen fonlar.
        """)]
    EMK = 2,

    [Description("""
        Exchange Traded Fund / Borsa Yatirim Fonu

        EN: Investment funds traded on stock exchanges, tracking an index, commodity, or basket of assets.
        TR: Bir endeksi, emtiayi veya varlik sepetini takip eden, borsada islem goren yatirim fonlari.
        """)]
    BYF = 3,

    [Description("""
        Real Estate Investment Fund / Gayrimenkul Yatirim Fonu

        EN: Funds that invest primarily in real estate assets and real estate-backed securities.
        TR: Oncelikli olarak gayrimenkul varliklarina ve gayrimenkul destekli menkul kiymetlere yatirim yapan fonlar.
        """)]
    GYF = 4,

    [Description("""
        Venture Capital Investment Fund / Girisim Sermayesi Yatirim Fonu

        EN: Funds that invest in early-stage and growth-stage companies, typically in technology and innovation sectors.
        TR: Genellikle teknoloji ve inovasyon sektorlerindeki erken ve buyume asamasindaki sirketlere yatirim yapan fonlar.
        """)]
    GSYF = 5
}
