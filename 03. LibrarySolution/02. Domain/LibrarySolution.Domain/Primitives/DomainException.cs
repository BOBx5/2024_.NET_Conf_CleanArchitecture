namespace LibrarySolution.Domain.Primitives;
public abstract class DomainException : Exception
{
    public Dictionary<string, object?>? Errors { get; }
    public DomainException(
        string message, 
        Dictionary<string, object?>? errors = null) 
        : base(message)
    {
        Errors = errors;
    }
}
