namespace BeforeCleanArchitecture.Dtos.Rents;

/// <summary>
/// 대여 조회 DTO
/// </summary>
public record class SelectRentDto
{
    /// <summary>
    /// 대여ID
    /// </summary>        
    public required string Id { get; set; }

    /// <summary>
    /// 도서ID
    /// </summary>        
    public required string BookId { get; set; }

    /// <summary>
    /// 유저ID
    /// </summary>        
    public required string UserId { get; set; }

    /// <summary>
    /// 대여일시
    /// </summary>        
    public required DateTime BorrowedAt { get; set; }

    /// <summary>
    /// 반납기한
    /// </summary>        
    public required DateTime DueDate { get; set; }

    /// <summary>
    /// 반납일시
    /// </summary>        
    public required DateTime? ReturnedAt { get; set; }

    /// <summary>
    /// 생성일시
    /// </summary>        
    public required DateTime CreatedAt { get; set; }

    /// <summary>
    /// 수정일시
    /// </summary>        
    public required DateTime? UpdatedAt { get; set; }

}
public static class SelectRentDtoExtension
{
    public static SelectRentDto ToDto(this Models.Rent rent)
    {
        return new SelectRentDto
        {
            Id = rent.Id,
            BookId = rent.BookId,
            UserId = rent.UserId,
            BorrowedAt = rent.BorrowedAt,
            DueDate = rent.DueDate,
            ReturnedAt = rent.ReturnedAt,
            CreatedAt = rent.CreatedAt,
            UpdatedAt = rent.UpdatedAt
        };
    }
}