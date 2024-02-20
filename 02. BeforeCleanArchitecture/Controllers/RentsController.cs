using BeforeCleanArchitecture.DbContexts;
using BeforeCleanArchitecture.Dtos.Rents;
using BeforeCleanArchitecture.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net.Mime;

namespace BeforeCleanArchitecture.Controllers;

/// <summary>
/// 대여 정보를 관리하는 엔드포인트입니다.
/// </summary>
[Route("api/[controller]")]
[ApiController]
[Produces(MediaTypeNames.Application.Json)]
public class RentsController : ControllerBase
{
    const int DefualtDueDates = 7;

    #region Constructor
    private readonly ILogger<RentsController> _logger;
    private readonly LibraryDbContextContext _context;
    public RentsController(ILogger<RentsController> logger, LibraryDbContextContext context)
    {
        _logger = logger;
        _context = context;
    }
    #endregion

    #region GET
    /// <summary>
    /// 대출 도서 목록을 요청합니다.
    /// </summary>
    [HttpGet]
    [ProducesResponseType<List<SelectRentDto>>(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetRents(
        [FromQuery] string? bookId,
        [FromQuery] string? userId)
    {
        _logger.LogInformation("대여 목록을 요청합니다.");

        if (!string.IsNullOrEmpty(bookId) && Guid.TryParse(bookId, out _))
            return BadRequest("올바른 도서ID 형식이 아닙니다.");

        if (!string.IsNullOrEmpty(userId) && Guid.TryParse(userId, out _))
            return BadRequest("올바른 유저ID 형식이 아닙니다.");

        var query = _context.Rents.AsQueryable().Where(rent => rent.ReturnedAt == null);

        if (!string.IsNullOrEmpty(bookId))
            query = query.Where(rent => rent.BookId == bookId);
        if (!string.IsNullOrEmpty(userId))
            query = query.Where(rent => rent.UserId == userId);

        var rents = await query
            .AsNoTracking()
            .ToListAsync();

        if (rents is null)
            return NotFound("입력하신 정보와 일치하는 대출 정보를 찾을 수 없습니다.");

        return Ok(rents.Select(i => i.ToDto()));
    }

    /// <summary>
    /// 도서 대출 정보를 요청합니다.
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType<SelectRentDto>(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetRent(string id)
    {
        _logger.LogInformation("대출 정보를 요청합니다.");

        if (!Guid.TryParse(id, out _))
            return BadRequest("올바른 대여ID 형식이 아닙니다.");

        var rent = await _context.Rents
            .AsNoTracking()
            .FirstOrDefaultAsync(rent => rent.Id == id);

        if (rent is null)
            return NotFound("입력하신 ID와 일치하는 대출 정보를 찾을 수 없습니다.");

        return Ok(rent.ToDto());
    }
    #endregion

    #region POST
    /// <summary>
    /// 도서를 대출합니다.
    /// </summary>
    [HttpPost]
    [ProducesResponseType<SelectRentDto>(StatusCodes.Status200OK)]
    public async Task<IActionResult> Create(CreateRentDto dto)
    {
        if (string.IsNullOrEmpty(dto.BookId))
            return BadRequest("도서ID를 입력해주세요.");

        if (string.IsNullOrEmpty(dto.UserId))
            return BadRequest("유저ID를 입력해주세요.");

        if (!Guid.TryParse(dto.BookId, out _))
            return BadRequest("올바른 도서ID 형식이 아닙니다.");

        if (!Guid.TryParse(dto.UserId, out _))
            return BadRequest("올바른 유저ID 형식이 아닙니다.");

        var book = await _context.Books.FirstOrDefaultAsync(book => book.Id == dto.BookId);
        if (book is null)
            return BadRequest("입력하신 ID와 일치하는 도서를 찾을 수 없습니다.");

        var user = await _context.Users.FirstOrDefaultAsync(user => user.Id == dto.UserId);
        if (user is null)
            return BadRequest("입력하신 ID와 일치하는 사용자를 찾을 수 없습니다.");

        if (book.Quantity <= 0)
            return BadRequest($"대출 가능한 도서보유수가 없습니다. ({book.Quantity})");

        book.Quantity -= 1;
        var rent = new Rent
        {
            Id = Guid.NewGuid().ToString(),
            DueDate = DateTime.Now.AddDays(DefualtDueDates),
            BookId = dto.BookId,
            UserId = dto.UserId,
            BorrowedAt = DateTime.Now,
            CreatedAt = DateTime.Now
        };
        await _context.Rents.AddAsync(rent);
        await _context.SaveChangesAsync();
        return Ok(rent.ToDto());
    }
    #endregion

    #region PUT
    /// <summary>
    /// 대출한 도서를 반환합니다.
    /// </summary>
    [HttpPut("{rentId}/return")]
    [ProducesResponseType<SelectRentDto>(StatusCodes.Status200OK)]
    public async Task<IActionResult> Return(string rentId)
    {
        if (!Guid.TryParse(rentId, out _))
            return BadRequest("올바른 대여ID 형식이 아닙니다.");

        var rent = await _context.Rents.FirstOrDefaultAsync(rent => rent.Id == rentId);
        if (rent is null)
            return NotFound("입력하신 ID와 일치하는 대출 정보를 찾을 수 없습니다.");

        if (rent.ReturnedAt != null)
            return BadRequest("이미 반납된 도서입니다.");

        var book = await _context.Books.FirstOrDefaultAsync(book => book.Id == rent.BookId);
        if (book is null)
            return BadRequest("입력하신 ID와 일치하는 도서를 찾을 수 없습니다.");

        book.Quantity += 1;
        rent.ReturnedAt = DateTime.UtcNow;
        rent.UpdatedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync();
        return Ok(rent.ToDto());
    }

    /// <summary>
    /// 대출한 도서의 기한을 연장합니다.
    /// </summary>
    [HttpPut("{rentId}/extend")]
    [ProducesResponseType<SelectRentDto>(StatusCodes.Status200OK)]
    public async Task<IActionResult> Extend(string rentId)
    {
        if (!Guid.TryParse(rentId, out _))
            return BadRequest("올바른 대여ID 형식이 아닙니다.");

        var rent = await _context.Rents.FirstOrDefaultAsync(rent => rent.Id == rentId);
        if (rent is null)
            return NotFound("입력하신 ID와 일치하는 대출 정보를 찾을 수 없습니다.");

        if (rent.ReturnedAt != null)
            return BadRequest("이미 반납된 도서입니다.");

        rent.DueDate = DateTime.Now.AddDays(DefualtDueDates);
        rent.UpdatedAt = DateTime.Now;
        await _context.SaveChangesAsync();
        return Ok(rent.ToDto());
    }
    #endregion
}
