using LibrarySolution.Domain.Aggregates.Rents.Exceptions;
using System.Diagnostics.CodeAnalysis;

namespace LibrarySolution.Domain.Aggregates.Rents.ValueObjects;

public record RentId
{
    #region Constructor
    public string Value { get; }
    private RentId(string value)
    {
        Value = value;
    }
    public override string ToString() => Value;
    #endregion

    #region Static Method
    public static RentId Create()
    {
        var guid = Guid.NewGuid().ToString();
        return new RentId($"{guid}");
    }
    public static RentId Parse(string value)
    {
        if (!Guid.TryParse(value, out Guid guid))
            throw new InvalidRentIdException(value);
        return new RentId($"{guid}");
    }
    public static bool TryParse(string value, [NotNullWhen(true)] out RentId? userId)
    {
        if (!Guid.TryParse(value, out var guid))
        {
            userId = null;
            return false;
        }
        userId = new RentId($"{guid}");
        return true;
    }
    #endregion
}
