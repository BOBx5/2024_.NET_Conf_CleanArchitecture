using System.ComponentModel.DataAnnotations;

namespace BeforeCleanArchitecture.Dtos.Books;

/// <summary>
/// 도서 수정 DTO
/// </summary>
public record class UpdateBookDto
{
    /// <summary>
    /// 도서고유ID
    /// </summary>summary>
    [Required]
    [StringLength(36)]
    public required string Id { get; set; }

    /// <summary>
    /// 도서 제목
    /// </summary>
    [StringLength(50)]
    public string? Title { get; set; }

    /// <summary>
    /// 도서 설명
    /// </summary>
    [StringLength(50)]
    public string? Description { get; set; }

    /// <summary>
    /// 도서 작가
    /// </summary>
    [StringLength(50)]
    public string? Author { get; set; }

    /// <summary>
    /// 보유 수량
    /// </summary>
    public int? Quantity { get; set; }
}
