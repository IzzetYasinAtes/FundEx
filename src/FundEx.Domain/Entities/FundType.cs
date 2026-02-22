using System.ComponentModel;
using FundEx.Domain.Common;

namespace FundEx.Domain.Entities;

[Description("""
    Fund Type / Fon Tipi

    EN: Represents the five main fund type classifications in the Turkish capital markets (TEFAS).
    TR: Turk sermaye piyasalarindaki (TEFAS) bes ana fon tipi siniflandirmasini temsil eder.
    """)]
public class FundType : Entity<Guid>
{
    [Description("""
        Fund Type Code / Fon Tipi Kodu

        EN: Short code identifying the fund type (YAT, EMK, BYF, GYF, GSYF).
        TR: Fon tipini tanimlayan kisa kod (YAT, EMK, BYF, GYF, GSYF).
        """)]
    public string Code { get; set; } = null!;

    [Description("""
        Fund Type Name / Fon Tipi Adi

        EN: Full English name of the fund type.
        TR: Fon tipinin tam Ingilizce adi.
        """)]
    public string Name { get; set; } = null!;

    [Description("""
        Fund Type Description / Fon Tipi Aciklamasi

        EN: Detailed bilingual description of the fund type.
        TR: Fon tipinin detayli iki dilli aciklamasi.
        """)]
    public string? Description { get; set; }

    public virtual ICollection<Fund> Funds { get; set; } = [];
    public virtual ICollection<FundCategory> Categories { get; set; } = [];
    public virtual ICollection<FundTitle> Titles { get; set; } = [];
    public virtual ICollection<Founder> Founders { get; set; } = [];
}
