namespace FundEx.WebApi.Controllers;
using FundEx.Application.Features.FundDailyData.Queries.GetFundDailyData;
using MediatR;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class FundDailyDataController : ControllerBase
{
    private readonly IMediator _mediator;

    public FundDailyDataController(IMediator mediator) => _mediator = mediator;

    [HttpGet("{fundCode}")]
    public async Task<IActionResult> Get(string fundCode, [FromQuery] DateTime startDate, [FromQuery] DateTime endDate,
        [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 25)
    {
        var result = await _mediator.Send(new GetFundDailyDataQuery(fundCode, startDate, endDate, pageNumber, pageSize));
        return Ok(result);
    }
}
