using System.ComponentModel.DataAnnotations;

namespace BeforeCleanArchitecture.Dtos.Rents;

/// <summary>
/// 대여 생성 DTO
/// </summary>
public record class CreateRentDto
{
    /// <summary>
    /// 도서고유ID
    /// </summary>
    [Required]
    [StringLength(36)]
    public required string BookId { get; set; }

    /// <summary>
    /// 유저고유ID
    /// </summary>
    [Required]
    [StringLength(36)]
    public required string UserId { get; set; }
}
