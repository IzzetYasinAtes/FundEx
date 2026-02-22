namespace FundEx.Application.Features.FundDailyData.Queries.GetFundDailyData;
using FluentValidation;

public class GetFundDailyDataValidator : AbstractValidator<GetFundDailyDataQuery>
{
    public GetFundDailyDataValidator()
    {
        RuleFor(x => x.FundCode).NotEmpty().MaximumLength(10);
        RuleFor(x => x.StartDate).LessThanOrEqualTo(x => x.EndDate);
        RuleFor(x => x.PageNumber).GreaterThan(0);
        RuleFor(x => x.PageSize).InclusiveBetween(1, 100);
    }
}
