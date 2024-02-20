using LibrarySolution.Domain.Aggregates.Books.ValueObjects;
using LibrarySolution.Domain.Aggregates.Rents.ValueObjects;
using LibrarySolution.Domain.Aggregates.Users.ValueObjects;

namespace LibrarySolution.Domain.Aggregates.Rents.DomainEvents;
public sealed record RentCreatedDomainEvent : DomainEvent
{
    public required RentId RentId { get; init; }
    public required BookId BookId { get; init; }
    public required UserId UserId { get; init; }
    public required DateTime BorrowedAt { get; init; }
}
