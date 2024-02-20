using LibrarySolution.Domain.Aggregates.Rents.ValueObjects;
using LibrarySolution.Domain.Aggregates.Users.ValueObjects;

namespace LibrarySolution.Domain.Aggregates.Users.Exceptions;
public sealed class UserNotFoundException : DomainException
{
    public string UserId { get; }
    public UserNotFoundException(UserId userId) : base(
        message: "해당하는 ID의 유저를 찾을 수 없습니다.",
        errors: new() { { nameof(UserId), userId.ToString() } })
    {
        UserId = userId.Value;
    }
}
