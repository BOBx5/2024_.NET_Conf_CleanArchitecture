using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace BeforeCleanArchitecture.Dtos.Users;

/// <summary>
/// 유저 조회 DTO 
/// </summary>
public record class SelectUserDto
{
    /// <summary>
    /// 유저고유ID
    /// </summary>
    [Unicode(false)]
    public required string Id { get; set; }

    /// <summary>
    /// 성명
    /// </summary>
    [StringLength(50)]
    public required string Name { get; set; }

    /// <summary>
    /// 이메일
    /// </summary>
    [StringLength(50)]
    public required string? Email { get; set; }

    /// <summary>
    /// 상태 (0: 가입대기, 1: 활동, 2: 정지)
    /// </summary>
    public required int Status { get; set; }

    /// <summary>
    /// 수정일시
    /// </summary>        
    public required DateTime? UpdatedAt { get; set; }

    /// <summary>
    /// 생성일시
    /// </summary>        
    public required DateTime CreatedAt { get; set; }
}
public static class SelectUserDtoExtension
{
    public static SelectUserDto ToDto(this Models.User user)
    {
        return new SelectUserDto
        {
            Id = user.Id,
            Name = user.Name,
            Email = user.Email,
            Status = user.Status,
            UpdatedAt = user.UpdatedAt,
            CreatedAt = user.CreatedAt
        };
    }
}
