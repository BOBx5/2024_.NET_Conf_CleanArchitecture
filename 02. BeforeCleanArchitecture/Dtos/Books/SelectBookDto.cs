using System.ComponentModel.DataAnnotations;

namespace BeforeCleanArchitecture.Dtos.Books;

/// <summary>
/// 도서 조회 DTO
/// </summary>
public record class SelectBookDto
{
    #region Property
    /// <summary>
    /// 도서고유ID
    /// </summary>
    [StringLength(36)]
    public required string Id { get; set; }

    /// <summary>
    /// 도서 제목
    /// </summary>
    [StringLength(50)]
    public required string Title { get; set; }

    /// <summary>
    /// 도서 설명
    /// </summary>
    [StringLength(50)]
    public required string Description { get; set; }

    /// <summary>
    /// 도서 작가
    /// </summary>
    [StringLength(50)]
    public required string Author { get; set; }

    /// <summary>
    /// 보유 수량
    /// </summary>
    public required int Quantity { get; set; }

    /// <summary>
    /// 수정일시
    /// </summary>
    public required DateTime? UpdatedAt { get; set; }

    /// <summary>
    /// 생성일시
    /// </summary>
    public required DateTime CreatedAt { get; set; }
    #endregion
}
public static class SelectBookDtoExtension
{
    public static SelectBookDto ToDto(this Models.Book book)
    {
        return new SelectBookDto
        {
            Id = book.Id,
            Title = book.Title,
            Description = book.Description,
            Author = book.Author,
            Quantity = book.Quantity,
            UpdatedAt = book.UpdatedAt,
            CreatedAt = book.CreatedAt
        };
    }
}
