namespace LibrarySolution.Domain.Aggregates.Rents.Exceptions;

internal class InvalidRentIdException : DomainException
{
    public string? TriedValue { get; }
    public InvalidRentIdException(string? triedValue) : base(
        message: "유효하지 않은 대여ID 유형입니다.",
        errors: new() { { nameof(TriedValue), triedValue } })
    {
        this.TriedValue = triedValue;
    }
}