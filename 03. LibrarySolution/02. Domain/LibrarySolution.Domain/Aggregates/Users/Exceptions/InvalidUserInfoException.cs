namespace LibrarySolution.Domain.Aggregates.Users.Exceptions;

internal class InvalidUserInfoException : DomainException
{
    public InvalidUserInfoException(string message = "유효하지 않은 유저 정보입니다.")
        : base(message)
    {
    }
}