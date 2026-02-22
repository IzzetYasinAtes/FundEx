namespace FundEx.WebApi.Controllers;
using FundEx.Application.Features.Founders.Queries.GetAllFounders;
using MediatR;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class FoundersController : ControllerBase
{
    private readonly IMediator _mediator;

    public FoundersController(IMediator mediator) => _mediator = mediator;

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] string? fundTypeCode = null)
    {
        var result = await _mediator.Send(new GetAllFoundersQuery(fundTypeCode));
        return Ok(result);
    }
}
