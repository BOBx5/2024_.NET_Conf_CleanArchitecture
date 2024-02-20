using BeforeCleanArchitecture.DbContexts;
using BeforeCleanArchitecture.Dtos.Books;
using BeforeCleanArchitecture.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net.Mime;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BeforeCleanArchitecture.Controllers;

/// <summary>
/// 도서 정보를 관리하는 엔드포인트입니다.
/// </summary>
[Route("api/[controller]")]
[ApiController]
[Produces(MediaTypeNames.Application.Json)]
public class BooksController : ControllerBase
{
    #region Constrcutor
    private readonly ILogger<BooksController> _logger;
    private readonly LibraryDbContextContext _context;
    public BooksController(ILogger<BooksController> logger, LibraryDbContextContext context)
    {
        _logger = logger;
        _context = context;
    }
    #endregion

    #region Get
    /// <summary>
    /// 도서 목록을 요청합니다.
    /// </summary>
    [HttpGet]
    [ProducesResponseType<List<SelectBookDto>>(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetBooks(
        [FromQuery] string? title,
        [FromQuery] string? author)
    {
        _logger.LogInformation("도서 목록을 요청합니다.");

        var query = _context.Books.AsQueryable();
        if (!string.IsNullOrEmpty(title))
            query = query.Where(book => book.Title.Contains(title));
        if (!string.IsNullOrEmpty(author))
            query = query.Where(book => book.Author.Contains(author));

        var books = await query.ToListAsync();
        return Ok(books.Select(i => i.ToDto()));
    }

    /// <summary>
    /// 도서 정보를 요청합니다.
    /// </summary>
    /// <param name="id">도서ID</param>
    /// <returns><see cref="SelectBookDto"/></returns>
    [HttpGet("{id}")]
    [ProducesResponseType<SelectBookDto>(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetBook(string id)
    {
        _logger.LogInformation("도서 정보를 요청합니다.");

        if (!Guid.TryParse(id, out _))
            return BadRequest("올바른 도서ID 형식이 아닙니다.");

        var book = await _context.Books
            .AsNoTracking()
            .FirstOrDefaultAsync(book => book.Id == id);
        if (book is null)
            return NotFound("입력하신 ID와 일치하는 도서를 찾을 수 없습니다.");

        return Ok(book.ToDto());
    }
    #endregion

    #region Post
    /// <summary>
    /// 도서를 생성합니다.
    /// </summary>
    /// <param name="dto">도서생성Dto</param>
    /// <returns></returns>
    [HttpPost]
    [ProducesResponseType<SelectBookDto>(StatusCodes.Status200OK)]
    public async Task<IActionResult> Create(CreateBookDto dto)
    {
        _logger.LogInformation("도서를 생성합니다.");

        if (dto is null)
            return BadRequest("입력하신 값이 올바르지 않습니다.");

        var book = new Book
        {
            Id = Guid.NewGuid().ToString(),
            Title = dto.Title,
            Description = dto.Description,
            Author = dto.Author,
            CreatedAt = DateTime.Now,
        };
        await _context.Books.AddAsync(book);
        await _context.SaveChangesAsync();
        return Ok(book);
    }
    #endregion

    #region Put
    /// <summary>
    /// 도서 정보를 수정합니다.
    /// </summary>
    [HttpPut("{id}")]
    [ProducesResponseType<SelectBookDto>(StatusCodes.Status200OK)]
    public async Task<IActionResult> Update(string id, UpdateBookDto dto)
    {
        _logger.LogInformation("도서 정보를 수정합니다.");

        if (dto is null)
            return BadRequest("입력하신 값이 올바르지 않습니다.");

        if (!Guid.TryParse(id, out _))
            return BadRequest("올바른 도서ID 형식이 아닙니다.");

        if (dto.Title is null && dto.Description is null && dto.Author is null && dto.Quantity is null)
            return BadRequest("수정할 값이 없습니다.");

        if (id != dto.Id)
            return BadRequest("요청하신 ID와 변경하려는 ID가 일치하지 않습니다.");

        if (dto.Quantity.HasValue && dto.Quantity.Value < 0)
            return BadRequest("수량을 0개 미만으로 지정할 수 없습니다.");

        var bookToUpdate = await _context.Books.FirstOrDefaultAsync(book => book.Id == id);
        if (bookToUpdate is null)
            return NotFound("입력하신 ID와 일치하는 도서를 찾을 수 없습니다.");

        if (!string.IsNullOrEmpty(dto.Title))
            bookToUpdate.Title = dto.Title;
        if (!string.IsNullOrEmpty(dto.Description))
            bookToUpdate.Description = dto.Description;
        if (!string.IsNullOrEmpty(dto.Author))
            bookToUpdate.Author = dto.Author;
        if (dto.Quantity.HasValue)
            bookToUpdate.Quantity = dto.Quantity.Value;

        bookToUpdate.UpdatedAt = DateTime.Now;
        await _context.SaveChangesAsync();
        return Ok(bookToUpdate.ToDto());
    }
    #endregion

    #region Delete
    /// <summary>
    /// 도서를 삭제합니다.
    /// </summary>
    [HttpDelete("{id}")]
    [ProducesResponseType<SelectBookDto>(StatusCodes.Status200OK)]
    public async Task<IActionResult> Delete(string id)
    {
        _logger.LogInformation("도서를 삭제합니다.");

        if (!Guid.TryParse(id, out _))
            return BadRequest("올바른 도서ID 형식이 아닙니다.");

        // 해당하는 ID의 도서를 대여 목록과 함께 가져옵니다.
        var bookToDelete = await _context.Books
            .Include(book => book.Rents)
            .FirstOrDefaultAsync(book => book.Id == id);

        if (bookToDelete is null)
            return NotFound("입력하신 ID와 일치하는 도서를 찾을 수 없습니다.");

        if (bookToDelete.Rents.Any())
            return BadRequest("대여 중인 도서는 삭제할 수 없습니다.");

        _context.Books.Remove(bookToDelete);
        await _context.SaveChangesAsync();
        return Ok(bookToDelete.ToDto());
    }
    #endregion
}
