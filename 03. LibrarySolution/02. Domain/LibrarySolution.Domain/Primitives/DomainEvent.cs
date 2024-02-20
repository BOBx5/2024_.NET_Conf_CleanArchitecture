namespace LibrarySolution.Domain.Primitives;


public abstract record DomainEvent : MediatR.INotification
{
    public Guid EventId { get; }
    protected DomainEvent()
    {
        EventId = Guid.NewGuid();
    }
}
