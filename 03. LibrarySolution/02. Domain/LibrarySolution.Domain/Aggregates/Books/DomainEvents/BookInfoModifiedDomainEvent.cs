using LibrarySolution.Domain.Aggregates.Books.ValueObjects;

namespace LibrarySolution.Domain.Aggregates.Books.DomainEvents;

public sealed record BookInfoModifiedDomainEvent : DomainEvent
{
    public required BookId BookId { get; init; }
    public required string? Title { get; init; }
    public required string? Description { get; init; }
    public required string? Author { get; init; }
}
