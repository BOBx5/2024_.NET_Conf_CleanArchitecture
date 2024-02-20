using LibrarySolution.Application.UseCases.Books.Commands;
using LibrarySolution.Application.UseCases.Books.Queries;
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
public class BooksController : ControllerBase
{
    #region Constructor
    private readonly ILogger<BooksController> _logger;
    private readonly IMediator _mediator;
    public BooksController(
        ILogger<BooksController> logger,
        IMediator mediator
        )
    {
        _logger = logger;
        _mediator = mediator;
    }
    #endregion

    #region GET
    /// <summary>
    /// 도서 목록을 조회합니다.
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(List<GetBookListQueryResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetBooks(
        [FromQuery] GetBookListQuery query)
    {
        var books = await _mediator.Send(query);
        return Ok(books);
    }
    /// <summary>
    /// 도서 정보를 조회합니다.
    /// </summary>
    [HttpGet("{bookId}")]
    [ProducesResponseType(typeof(GetBookInfoQueryResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetBookInfo(
        [FromRoute]
        [DefaultValue(GuidHelper.EmptyGuidWithHypen)]
        string bookId)
    {
        GetBookInfoQuery query = new() { BookId = bookId };
        GetBookInfoQueryResponse book = await _mediator.Send(query);
        return Ok(book);
    }
    #endregion

    #region POST
    /// <summary>
    /// 도서를 생성합니다.
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(CreateBookCommandResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> CreateBook(
        [FromBody] CreateBookCommand command)
    {
        var response = await _mediator.Send(command);
        return Ok(response);
    }
    #endregion

    #region PUT
    /// <summary>
    /// 도서 정보를 수정합니다.
    /// </summary>
    [HttpPut]
    [ProducesResponseType(typeof(ModifyBookInfoCommandResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> ModifyBookInfo(
        [FromBody] ModifyBookInfoCommand command)
    {
        var response = await _mediator.Send(command);
        return Ok(response);
    }
    #endregion

    #region DELETE
    /// <summary>
    /// 도서를 삭제합니다.
    /// </summary>
    [HttpDelete]
    [ProducesResponseType(typeof(RemoveBookCommandResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> RemoveBook(
        [FromBody] RemoveBookCommand command)
    {
        var response = await _mediator.Send(command);
        return Ok(response);
    }
    #endregion
}
