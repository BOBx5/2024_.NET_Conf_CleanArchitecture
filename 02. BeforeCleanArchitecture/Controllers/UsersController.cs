using BeforeCleanArchitecture.DbContexts;
using BeforeCleanArchitecture.Dtos.Users;
using BeforeCleanArchitecture.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net.Mime;
using System.Text.RegularExpressions;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BeforeCleanArchitecture.Controllers;

/// <summary>
/// 유저 정보를 관리하는 엔드포인트입니다.
/// </summary>
[Route("api/[controller]")]
[ApiController]
[Produces(MediaTypeNames.Application.Json)]
public class UsersController : ControllerBase
{
    private static readonly Regex EmailRegex = new Regex(@"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$");

    #region Constructor
    private readonly ILogger<UsersController> _logger;
    private readonly LibraryDbContextContext _context;
    public UsersController(ILogger<UsersController> logger, LibraryDbContextContext context)
    {
        _logger = logger;
        _context = context;
    }
    #endregion

    #region Get
    /// <summary>
    /// 사용자 목록을 요청합니다.
    /// </summary>
    /// <param name="name">검색어로 포함시킬 이름</param>
    /// <param name="email">검색어로 포함시킬 이메일</param>
    /// <returns></returns>
    [HttpGet]
    [ProducesResponseType<List<SelectUserDto>>(StatusCodes.Status200OK)]
    [ProducesResponseType<SelectUserDto>(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetUsers(
        [FromQuery] string? name,
        [FromQuery] string? email)
    {
        _logger.LogInformation("사용자 목록을 요청합니다.");

        var query = _context.Users.AsQueryable();
        if (!string.IsNullOrEmpty(name))
            query = query.Where(user => user.Name.Contains(name));
        if (!string.IsNullOrEmpty(email))
            query = query.Where(user => user.Email != null && user.Email.Contains(email));

        var users = await query.ToListAsync();
        return Ok(users.Select(i => i.ToDto()));
    }

    /// <summary>
    /// 사용자 정보를 요청합니다.
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType<SelectUserDto>(StatusCodes.Status200OK)]
    [ProducesResponseType<SelectUserDto>(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetUser(string id)
    {
        _logger.LogInformation("사용자를 정보를 요청합니다.");

        if (!Guid.TryParse(id, out _))
            return BadRequest("올바른 유저ID 형식이 아닙니다.");

        var user = await _context.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(user => user.Id == id);

        if (user is null)
            return NotFound("입력하신 ID와 일치하는 사용자를 찾을 수 없습니다.");

        return Ok(user.ToDto());
    }
    #endregion

    #region Post
    /// <summary>
    /// 사용자를 생성합니다.
    /// </summary>
    [HttpPost]
    [ProducesResponseType<SelectUserDto>(StatusCodes.Status200OK)]
    public async Task<IActionResult> Create(CreateUserDto dto)
    {
        if (dto is null)
            return BadRequest("입력하신 값이 올바르지 않습니다.");

        if (string.IsNullOrEmpty(dto.Name))
            return BadRequest("이름은 필수 입력값입니다.");

        if (!string.IsNullOrEmpty(dto.Email) && !EmailRegex.IsMatch(dto.Email))
            return BadRequest("이메일 형식이 잘못되었습니다.");

        var user = new User
        {
            Id = Guid.NewGuid().ToString(),
            Name = dto.Name,
            Email = dto.Email,
            Status = 1,
            CreatedAt = DateTime.Now
        };
        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();

        return Ok(new SelectUserDto
        {
            Id = user.Id,
            Name = user.Name,
            Email = user.Email,
            Status = user.Status,
            CreatedAt = user.CreatedAt,
            UpdatedAt = user.UpdatedAt
        });
    }
    #endregion

    #region Put
    /// <summary>
    /// 사용자 정보를 수정합니다.
    /// </summary>
    [HttpPut("{id}")]
    [ProducesResponseType<SelectUserDto>(StatusCodes.Status200OK)]
    public async Task<IActionResult> Update(string id, UpdateUserDto dto)
    {
        _logger.LogInformation("사용자 정보를 수정합니다.");

        if (dto is null)
            return BadRequest("입력하신 값이 올바르지 않습니다.");

        if (!Guid.TryParse(id, out _))
            return BadRequest("올바른 유저ID 형식이 아닙니다.");

        if (id != dto.Id)
            return BadRequest("요청하신 ID와 변경하려는 ID가 일치하지 않습니다.");

        if (string.IsNullOrEmpty(dto.Name) && string.IsNullOrEmpty(dto.Email))
            return BadRequest("수정할 값이 없습니다.");

        if (!string.IsNullOrEmpty(dto.Email) && !Regex.IsMatch(dto.Email, @"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$"))
            return BadRequest("이메일 형식이 잘못되었습니다.");

        var user = await _context.Users.FirstOrDefaultAsync(user => user.Id == id);
        if (user is null)
            return NotFound("입력하신 ID와 일치하는 사용자를 찾을 수 없습니다.");

        if (!string.IsNullOrEmpty(dto.Name))
            user.Name = dto.Name;
        if (!string.IsNullOrEmpty(dto.Email))
            user.Email = dto.Email;

        user.UpdatedAt = DateTime.Now;
        await _context.SaveChangesAsync();
        return Ok(user.ToDto());
    }
    #endregion

    #region Delete
    /// <summary>
    /// 사용자를 삭제합니다.
    /// </summary>
    [HttpDelete("{id}")]
    [ProducesResponseType<SelectUserDto>(StatusCodes.Status200OK)]
    public async Task<IActionResult> Delete(string id)
    {
        _logger.LogInformation("사용자를 삭제합니다.");

        if (!Guid.TryParse(id, out _))
            return BadRequest("올바른 유저ID 형식이 아닙니다.");

        var user = await _context.Users
            .Include(user => user.Rents)
            .FirstOrDefaultAsync(user => user.Id == id);
        if (user is null)
            return NotFound("입력하신 ID와 일치하는 사용자를 찾을 수 없습니다.");

        if (user.Rents.Any())
            return BadRequest("대여 중인 도서가 있어 삭제할 수 없습니다.");

        _context.Users.Remove(user);
        await _context.SaveChangesAsync();
        return Ok(user.ToDto());
    }
    #endregion
}
