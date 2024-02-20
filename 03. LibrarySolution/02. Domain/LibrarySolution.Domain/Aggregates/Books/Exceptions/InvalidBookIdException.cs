namespace LibrarySolution.Domain.Aggregates.Books.Exceptions;

public sealed class InvalidBookIdException : DomainException
{
    public string? TriedValue { get; }
    public InvalidBookIdException(string? triedValue) : base(
        message: "유효하지 않은 도서ID 유형입니다.",
        errors: new() { { nameof(TriedValue), triedValue } })
    {
        this.TriedValue = triedValue;
    }
}