namespace FundEx.Domain.Tests.Entities;

using FluentAssertions;
using FundEx.Domain.Common;
using FundEx.Domain.Entities;

public class FundTests
{
    [Fact]
    public void Fund_Should_Set_Properties()
    {
        var fund = new Fund
        {
            Id = Guid.NewGuid(),
            Code = "AAK",
            Title = "ATA PORTFOY COKLU VARLIK DEGISKEN FON",
            FundTypeId = Guid.NewGuid()
        };

        fund.Code.Should().Be("AAK");
        fund.Title.Should().NotBeEmpty();
        fund.FounderId.Should().BeNull();
    }

    [Fact]
    public void Fund_CheckRule_Should_Throw_When_Rule_IsBroken()
    {
        var fund = new Fund { Id = Guid.NewGuid(), Code = "TST", Title = "Test Fund", FundTypeId = Guid.NewGuid() };
        var brokenRule = new TestBrokenRule();

        var act = () => fund.CheckRule(brokenRule);

        act.Should().Throw<BusinessRuleValidationException>()
            .Which.BrokenRule.Should().Be(brokenRule);
    }

    [Fact]
    public void Fund_CheckRule_Should_Not_Throw_When_Rule_IsNotBroken()
    {
        var fund = new Fund { Id = Guid.NewGuid(), Code = "TST", Title = "Test Fund", FundTypeId = Guid.NewGuid() };
        var validRule = new TestValidRule();

        var act = () => fund.CheckRule(validRule);

        act.Should().NotThrow();
    }

    private class TestBrokenRule : IBusinessRule
    {
        public string Message => "Rule is broken";
        public bool IsBroken() => true;
    }

    private class TestValidRule : IBusinessRule
    {
        public string Message => "Rule is valid";
        public bool IsBroken() => false;
    }
}
