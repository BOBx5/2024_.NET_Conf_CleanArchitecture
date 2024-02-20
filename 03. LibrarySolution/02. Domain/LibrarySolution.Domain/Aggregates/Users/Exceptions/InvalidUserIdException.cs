namespace LibrarySolution.Domain.Aggregates.Users.Exceptions;

internal class InvalidUserIdException : DomainException
{
    public string? TriedValue { get; }
    public InvalidUserIdException(string? triedValue) : base(
        message: "유효하지 않은 유저ID 유형입니다.",
        errors: new() { { nameof(TriedValue), triedValue } })
    {
        this.TriedValue = triedValue;
    }
}