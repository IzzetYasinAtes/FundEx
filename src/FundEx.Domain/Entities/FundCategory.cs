using System.ComponentModel;
using FundEx.Domain.Common;

namespace FundEx.Domain.Entities;

[Description("""
    Fund Category / Fon Turu

    EN: Represents the sub-category of a fund type. For YAT: umbrella fund types (sfonTuru).
        For EMK/BYF: fund type codes (fonTurKod). GYF/GSYF have no categories.
    TR: Bir fon tipinin alt kategorisini temsil eder. YAT icin: semsiye fon turleri (sfonTuru).
        EMK/BYF icin: fon tur kodlari (fonTurKod). GYF/GSYF de kategori yoktur.
    """)]
public class FundCategory : Entity<Guid>
{
    [Description("""
        Category Code / Kategori Kodu

        EN: Numeric code identifying the category (e.g., 100, 101, 26, 37).
        TR: Kategoriyi tanimlayan sayisal kod (ornegin 100, 101, 26, 37).
        """)]
    public string Code { get; set; } = null!;

    [Description("""
        Category Name / Kategori Adi

        EN: Display name of the category (e.g., 'Borclanma Araclari Semsiye Fonu').
        TR: Kategorinin gorunen adi (ornegin 'Borclanma Araclari Semsiye Fonu').
        """)]
    public string Name { get; set; } = null!;

    public Guid FundTypeId { get; set; }
    public virtual FundType FundType { get; set; } = null!;
}
