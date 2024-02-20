using System.ComponentModel.DataAnnotations;

namespace BeforeCleanArchitecture.Dtos.Users;

/// <summary>
/// 유저 생성 DTO
/// </summary>
public class CreateUserDto
{
    /// <summary>
    /// 성명
    /// </summary>
    [Required]
    [StringLength(50)]
    public required string Name { get; set; }

    /// <summary>
    /// 이메일
    /// </summary>
    [StringLength(50)]
    public string? Email { get; set; }
}
