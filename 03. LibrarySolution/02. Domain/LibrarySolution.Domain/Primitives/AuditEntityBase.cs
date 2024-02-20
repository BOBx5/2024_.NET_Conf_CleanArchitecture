namespace LibrarySolution.Domain.Primitives;
public abstract class AuditEntityBase : EntityBase
{
    public DateTime? UpdatedAt { get; private set; }
    public DateTime CreatedAt { get; private set; }

    public void SetUpdated(DateTime updatedAt)
    {
        UpdatedAt = updatedAt;
    }
    public void SetCreated(DateTime createdAt)
    {
        CreatedAt = createdAt;
    }

}
