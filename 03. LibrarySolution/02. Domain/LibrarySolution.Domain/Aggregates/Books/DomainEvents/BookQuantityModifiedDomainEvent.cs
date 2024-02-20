using LibrarySolution.Domain.Aggregates.Books.ValueObjects;

namespace LibrarySolution.Domain.Aggregates.Books.DomainEvents;

public sealed record BookQuantityModifiedDomainEvent : DomainEvent
{
    public required BookId BookId { get; init; }
    public required int ChangedQuantity { get; init; }
}
