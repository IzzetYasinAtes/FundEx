using System.ComponentModel;
using FundEx.Domain.Common;

namespace FundEx.Domain.Entities;

[Description("""
    Fund Title / Fon Unvani

    EN: Represents the title/label used for filtering funds. Source varies by fund type:
        YAT/BYF/GYF/GSYF -> fonUnvanGetir API 'tanim' field.
        EMK -> fonGrupGetir API 'fongrupaciklama' field.
    TR: Fonlari filtrelemek icin kullanilan unvan/etiketi temsil eder. Kaynak fon tipine gore degisir:
        YAT/BYF/GYF/GSYF -> fonUnvanGetir API 'tanim' alani.
        EMK -> fonGrupGetir API 'fongrupaciklama' alani.
    """)]
public class FundTitle : Entity<Guid>
{
    [Description("""
        Group Code / Grup Kodu

        EN: Numeric group code, only used for EMK pension funds (fonGrubu value like 75, 77, 78 etc.).
        TR: Sayisal grup kodu, sadece EMK emeklilik fonlari icin kullanilir (fonGrubu degeri: 75, 77, 78 vb.).
        """)]
    public int? GroupCode { get; set; }

    [Description("""
        Title Name / Unvan Adi

        EN: The display name of the fund title (e.g., 'Altin', 'Borclanma Araclari', 'Gelir Amacli Fon').
        TR: Fon unvaninin gorunen adi (ornegin 'Altin', 'Borclanma Araclari', 'Gelir Amacli Fon').
        """)]
    public string Name { get; set; } = null!;

    public Guid FundTypeId { get; set; }
    public virtual FundType FundType { get; set; } = null!;
}
