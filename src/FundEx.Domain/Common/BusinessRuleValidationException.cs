namespace FundEx.Domain.Common;

public sealed class BusinessRuleValidationException : Exception
{
    public IBusinessRule BrokenRule { get; }
    public string Details { get; }

    public BusinessRuleValidationException(IBusinessRule brokenRule)
        : base(brokenRule.Message)
    {
        BrokenRule = brokenRule;
        Details = brokenRule.Message;
    }

    public override string ToString() => $"{BrokenRule.GetType().Name}: {BrokenRule.Message}";
}
