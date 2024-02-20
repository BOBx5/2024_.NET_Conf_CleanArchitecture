using System.ComponentModel.DataAnnotations;

namespace BeforeCleanArchitecture.Dtos.Books;

/// <summary>
/// 도서 생성 DTO
/// </summary>
public record class CreateBookDto
{
    /// <summary>
    /// 도서 제목
    /// </summary>
    [Required]
    [StringLength(50)]
    public required string Title { get; set; }

    /// <summary>
    /// 도서 설명
    /// </summary>
    [Required]
    [StringLength(50)]
    public required string Description { get; set; }

    /// <summary>
    /// 도서 작가
    /// </summary>
    [Required]
    [StringLength(50)]
    public required string Author { get; set; }
}
