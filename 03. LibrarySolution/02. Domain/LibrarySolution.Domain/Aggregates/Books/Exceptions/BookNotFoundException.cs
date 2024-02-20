using LibrarySolution.Domain.Aggregates.Books.ValueObjects;
using LibrarySolution.Domain.Aggregates.Users.ValueObjects;

namespace LibrarySolution.Domain.Aggregates.Books.Exceptions;

public sealed class BookNotFoundException : DomainException
{
    public string BookId { get; }
    public BookNotFoundException(BookId bookId) : base(
        message: "해당하는 ID의 도서를 찾을 수 없습니다.",
        errors: new() { 
            { nameof(BookId), bookId.ToString() } 
        })
    {
        BookId = bookId.ToString();
    }
}
