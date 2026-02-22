using System.ComponentModel;
using FundEx.Domain.Common;

namespace FundEx.Domain.Entities;

[Description("""
    Founder / Fon Kurucusu

    EN: Represents a fund management company (portfolio management firm) that creates and manages funds.
    TR: Fonlari olusturan ve yoneten portfoy yonetim sirketini temsil eder.
    """)]
public class Founder : Entity<Guid>
{
    [Description("""
        Founder Code / Kurucu Kodu

        EN: Short code identifying the founder company (e.g., 'AKP', 'ZPY', 'ISP').
        TR: Kurucu sirketini tanimlayan kisa kod (ornegin 'AKP', 'ZPY', 'ISP').
        """)]
    public string Code { get; set; } = null!;

    [Description("""
        Founder Title / Kurucu Unvani

        EN: Full official name of the portfolio management company.
        TR: Portfoy yonetim sirketinin tam resmi adi.
        """)]
    public string Title { get; set; } = null!;

    [Description("""
        Fund Type Code / Fon Tipi Kodu

        EN: The fund type code this founder belongs to (YAT, EMK, BYF, GYF, GSYF).
            Stored from the request parameter, not from the API response value (which returns 'F', '0', '1' etc.).
        TR: Bu kurucunun ait oldugu fon tipi kodu (YAT, EMK, BYF, GYF, GSYF).
            API yanit degerinden degil (F, 0, 1 vb.), istek parametresinden saklanir.
        """)]
    public string FundTypeCode { get; set; } = null!;

    public Guid FundTypeId { get; set; }
    public virtual FundType FundType { get; set; } = null!;

    public virtual ICollection<Fund> Funds { get; set; } = [];
}
