using System.ComponentModel.DataAnnotations;

namespace BeforeCleanArchitecture.Dtos.Users;

/// <summary>
/// 유저 수정 DTO
/// </summary>
public class UpdateUserDto
{
    /// <summary>
    /// 유저고유ID
    /// </summary>
    [Required]
    [StringLength(36)]
    public required string Id { get; set; }

    /// <summary>
    /// 성명
    /// </summary>
    [StringLength(50)]
    public string? Name { get; set; }

    /// <summary>
    /// 이메일
    /// </summary>
    [StringLength(50)]
    public string? Email { get; set; }
}
