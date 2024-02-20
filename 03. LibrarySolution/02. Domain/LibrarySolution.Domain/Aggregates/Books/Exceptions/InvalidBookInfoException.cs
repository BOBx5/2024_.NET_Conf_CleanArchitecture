namespace LibrarySolution.Domain.Aggregates.Books.Exceptions;

public sealed class InvalidBookInfoException : DomainException
{
    public InvalidBookInfoException(string message = "유효하지 않은 도서 정보입니다.")
        : base(message: message)
    {
    }
}