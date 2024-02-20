using LibrarySolution.Domain.Aggregates.Users.Enums;

namespace LibrarySolution.Domain.Aggregates.Users.Exceptions;

internal class InvalidUserStatusException : DomainException
{
    public UserStatus Status { get; }
    public InvalidUserStatusException(string message, UserStatus status)
        : base(message)
    {
        this.Status = status;
    }
}