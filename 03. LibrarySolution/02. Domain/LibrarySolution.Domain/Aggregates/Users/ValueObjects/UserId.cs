using LibrarySolution.Domain.Aggregates.Users.Exceptions;
using System.Diagnostics.CodeAnalysis;

namespace LibrarySolution.Domain.Aggregates.Users.ValueObjects;

public record UserId
{
    #region Constructor
    public string Value { get; }
    private UserId(string value)
    {
        Value = value;
    }
    public override string ToString() => Value;
    #endregion

    #region Static Method
    public static UserId Create()
    {
        var guid = Guid.NewGuid();
        return new UserId($"{guid}");
    }
    public static UserId Parse(string value)
    {
        if (!Guid.TryParse(value, out Guid guid))
            throw new InvalidUserIdException(value);
        return new UserId($"{guid}");
    }
    public static bool TryParse(string value, [NotNullWhen(true)] out UserId? userId)
    {
        if (!Guid.TryParse(value, out var guid))
        {
            userId = null;
            return false;
        }
        userId = new UserId($"{guid}");
        return true;
    }
    #endregion
}