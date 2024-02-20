using LibrarySolution.Domain.Aggregates.Books.ValueObjects;

namespace LibrarySolution.Domain.Aggregates.Books.Exceptions;

public sealed class BookOutOfStockException : DomainException
{
    public string BookId { get; }
    public BookOutOfStockException(BookId bookId) : base(
        message: "해당하는 도서의 재고가 없습니다.",
        errors: new() { { nameof(BookId), bookId.ToString() } })
    {
        BookId = bookId.ToString();
    }
}
