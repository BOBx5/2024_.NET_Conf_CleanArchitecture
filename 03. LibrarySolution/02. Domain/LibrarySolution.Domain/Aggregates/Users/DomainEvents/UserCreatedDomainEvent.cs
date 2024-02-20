using LibrarySolution.Domain.Aggregates.Users.ValueObjects;

namespace LibrarySolution.Domain.Aggregates.Users.DomainEvents;

public sealed record UserCreatedDomainEvent : DomainEvent
{
    public UserId UserId { get; }
    public string Name { get; }
    public string Email { get; }
    public UserCreatedDomainEvent(UserId userId, string name, string email)
    {
        UserId = userId;
        Name = name;
        Email = email;
    }
}