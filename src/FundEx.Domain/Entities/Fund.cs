using System.ComponentModel;
using FundEx.Domain.Common;

namespace FundEx.Domain.Entities;

[Description("""
    Fund / Fon

    EN: Represents an individual investment fund registered in TEFAS. Each fund has a unique code and belongs to a fund type.
    TR: TEFAS'a kayitli bireysel bir yatirim fonunu temsil eder. Her fonun benzersiz bir kodu vardir ve bir fon tipine aittir.
    """)]
public class Fund : Entity<Guid>
{
    [Description("""
        Fund Code / Fon Kodu

        EN: Unique short code identifying the fund (e.g., 'AAK', 'ZGD', 'BLH'). This is the primary business identifier.
        TR: Fonu tanimlayan benzersiz kisa kod (ornegin 'AAK', 'ZGD', 'BLH'). Bu birincil is tanimlayicisidir.
        """)]
    public string Code { get; set; } = null!;

    [Description("""
        Fund Title / Fon Unvani

        EN: Full official name of the fund (e.g., 'ATA PORTFOY COKLU VARLIK DEGISKEN FON').
        TR: Fonun tam resmi adi (ornegin 'ATA PORTFOY COKLU VARLIK DEGISKEN FON').
        """)]
    public string Title { get; set; } = null!;

    public Guid FundTypeId { get; set; }
    public virtual FundType FundType { get; set; } = null!;

    public Guid? FounderId { get; set; }
    public virtual Founder? Founder { get; set; }

    public virtual ICollection<FundDailyData> DailyData { get; set; } = [];

    public void CheckRule(IBusinessRule rule)
    {
        if (rule.IsBroken())
        {
            throw new BusinessRuleValidationException(rule);
        }
    }
}
