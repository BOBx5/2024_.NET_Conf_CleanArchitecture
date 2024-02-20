namespace LibrarySolution.Domain.Primitives;

public abstract class EntityBase
{
    private readonly List<DomainEvent> _domainEvents = new();
    protected EntityBase() { }

    public ICollection<DomainEvent> GetDomainEvents() 
        => _domainEvents;
    protected void Raise(DomainEvent domainEvent) 
        => _domainEvents.Add(domainEvent);
    /// <summary>
    /// <b>반드시 비워주어야 하는 이유</b>:<br/>
    /// 도메인 이벤트가 Publish 되면서 경우에 따라 발생하는 SideEffect로 인해
    /// 이미 처리된 도메인 이벤트가  중복으로 처리될 수 있기 때문에
    /// </summary>
    public void ClearDomainEvents() 
        => _domainEvents.Clear();
}
