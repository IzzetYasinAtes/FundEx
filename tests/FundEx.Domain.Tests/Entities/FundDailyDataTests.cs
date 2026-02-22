namespace FundEx.Domain.Tests.Entities;

using FluentAssertions;
using FundEx.Domain.Entities;

public class FundDailyDataTests
{
    [Fact]
    public void FundDailyData_Should_Have_Default_Null_Distribution_Values()
    {
        var data = new FundDailyData();

        data.Stock.Should().BeNull();
        data.GovernmentBond.Should().BeNull();
        data.ReverseRepo.Should().BeNull();
        data.PreciousMetals.Should().BeNull();
        data.FuturesCashCollateral.Should().BeNull();
    }

    [Fact]
    public void FundDailyData_Should_Set_General_Info_Properties()
    {
        var data = new FundDailyData
        {
            Id = Guid.NewGuid(),
            FundId = Guid.NewGuid(),
            Date = new DateTime(2026, 2, 20),
            Price = 34.468396m,
            ShareCount = 1153385,
            InvestorCount = 857,
            PortfolioSize = 39755331.09m,
            ExchangeBulletinPrice = null
        };

        data.Price.Should().Be(34.468396m);
        data.ShareCount.Should().Be(1153385);
        data.InvestorCount.Should().Be(857);
        data.PortfolioSize.Should().Be(39755331.09m);
    }
}
