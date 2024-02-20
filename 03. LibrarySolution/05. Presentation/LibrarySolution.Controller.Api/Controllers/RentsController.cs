using LibrarySolution.Application.UseCases.Rents.Commands;
using LibrarySolution.Application.UseCases.Rents.Queries;
using LibrarySolution.Shared.Helpers;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
using System.Net.Mime;

namespace LibrarySolution.Controller.Api.Controllers;
[Route("api/[controller]")]
[ApiController]
[Consumes(MediaTypeNames.Application.Json)]
[Produces(MediaTypeNames.Application.Json)]
public class RentsController : ControllerBase
{
    #region Constructor
    private readonly ILogger<RentsController> _logger;
    private readonly IMediator _mediator;
    public RentsController(
        ILogger<RentsController> logger,
        IMediator mediator)
    {
        _logger = logger;
        _mediator = mediator;
    }
    #endregion

    #region GET
    /// <summary>
    /// 대여 목록을 조회합니다.
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(List<GetRentListQueryResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetRents(
        [FromQuery] GetRentListQuery query)
    {
        var rents = await _mediator.Send(query);
        return Ok(rents);
    }
    /// <summary>
    /// 대여 정보를 조회합니다.
    /// </summary>
    [HttpGet("{rentId}")]
    [ProducesResponseType(typeof(GetRentInfoQueryResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetRentInfo(
        [FromRoute]
        [DefaultValue(GuidHelper.EmptyGuidWithHypen)]
        string rentId)
    {
        GetRentInfoQuery query = new() { RentId = rentId };
        var rent = await _mediator.Send(query);
        return Ok(rent);
    }
    #endregion

    #region POST
    /// <summary>
    /// 대여정보를 생성합니다.
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(CreateRentCommandResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> CreateRent(
        [FromBody] CreateRentCommand command)
    {
        var response = await _mediator.Send(command);
        return Ok(response);
    }
    #endregion

    #region PUT
    /// <summary>
    /// 대여를 연장합니다.
    /// </summary>
    [HttpPut("{rentId}/Extend")]
    [ProducesResponseType(typeof(ExtendRentCommandResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> Extend(
        [FromRoute]
        [DefaultValue(GuidHelper.EmptyGuidWithHypen)]
        string rentId)
    {
        ExtendRentCommand command = new() { RentId = rentId };
        var response = await _mediator.Send(command);
        return Ok(response);
    }
    /// <summary>
    /// 대여를 반납합니다.
    /// </summary>
    [HttpPut("{rentId}/Return")]
    [ProducesResponseType(typeof(ReturnRentCommandResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> Return(
        [FromRoute]
        [DefaultValue(GuidHelper.EmptyGuidWithHypen)]
        string rentId)
    {
        ReturnRentCommand command = new() { RentId = rentId };
        var response = await _mediator.Send(command);
        return Ok(response);
    }
    #endregion
}
