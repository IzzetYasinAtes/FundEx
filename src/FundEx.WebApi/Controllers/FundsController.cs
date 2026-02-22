namespace FundEx.WebApi.Controllers;
using FundEx.Application.Features.Funds.Queries.GetAllFunds;
using FundEx.Application.Features.Funds.Queries.GetFundByCode;
using MediatR;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class FundsController : ControllerBase
{
    private readonly IMediator _mediator;

    public FundsController(IMediator mediator) => _mediator = mediator;

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 25,
        [FromQuery] string? fundTypeCode = null, [FromQuery] string? searchText = null)
    {
        var result = await _mediator.Send(new GetAllFundsQuery(pageNumber, pageSize, fundTypeCode, searchText));
        return Ok(result);
    }

    [HttpGet("{code}")]
    public async Task<IActionResult> GetByCode(string code)
    {
        var result = await _mediator.Send(new GetFundByCodeQuery(code));
        return Ok(result);
    }
}
