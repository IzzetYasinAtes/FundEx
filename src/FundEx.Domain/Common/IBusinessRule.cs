namespace FundEx.Domain.Common;

public interface IBusinessRule
{
    string Message { get; }
    bool IsBroken();
}
