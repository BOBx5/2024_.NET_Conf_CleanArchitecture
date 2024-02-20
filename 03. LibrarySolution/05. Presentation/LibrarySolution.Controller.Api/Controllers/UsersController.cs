using LibrarySolution.Application.UseCases.Users.Commands;
using LibrarySolution.Application.UseCases.Users.Queries;
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
public class UsersController : ControllerBase
{
    #region Constructor
    private readonly ILogger<UsersController> _logger;
    private readonly IMediator _mediator;
    public UsersController(
        ILogger<UsersController> logger,
        IMediator mediator)
    {
        _logger = logger;
        _mediator = mediator;
    }
    #endregion

    #region GET
    /// <summary>
    /// 유저 목록을 조회합니다.
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(List<GetUserListQueryResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetUsers(
        [FromQuery] GetUserListQuery query)
    {
        var users = await _mediator.Send(query);
        return Ok(users);
    }

    /// <summary>
    /// 유저 정보를 조회합니다.
    /// </summary>
    [HttpGet("{userId}")]
    [ProducesResponseType(typeof(GetUserInfoQueryResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetUserInfo(
        [FromRoute]
        [DefaultValue(GuidHelper.EmptyGuidWithHypen)]
        string userId)
    {
        var query = new GetUserInfoQuery { UserId = userId };
        var user = await _mediator.Send(query);
        return Ok(user);
    }
    #endregion

    #region POST
    /// <summary>
    /// 유저를 생성합니다.
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(CreateUserCommandResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> CreateUser(
        [FromBody] CreateUserCommand command)
    {
        var response = await _mediator.Send(command);
        return Ok(response);
    }
    #endregion

    #region PUT
    /// <summary>
    /// 유저 정보를 수정합니다.
    /// </summary>
    [HttpPut]
    [ProducesResponseType(typeof(ModifyUserInfoCommandResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> ModifyUserInfo(
        [FromBody] ModifyUserInfoCommand command)
    {
        var response = await _mediator.Send(command);
        return Ok(response);
    }
    #endregion

    #region DELETE
    /// <summary>
    /// 유저를 삭제합니다.
    /// </summary>
    [HttpDelete]
    [ProducesResponseType(typeof(RemoveUserCommandResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> RemoveUser(
        [FromBody] RemoveUserCommand command)
    {
        var response = await _mediator.Send(command);
        return Ok(response);
    }
    #endregion
}
