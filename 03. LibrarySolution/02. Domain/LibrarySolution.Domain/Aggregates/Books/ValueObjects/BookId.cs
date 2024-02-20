using LibrarySolution.Domain.Aggregates.Books.Exceptions;
using System.Diagnostics.CodeAnalysis;

namespace LibrarySolution.Domain.Aggregates.Books.ValueObjects;

public record BookId
{
    #region Constructor
    public string Value { get; }
    private BookId(string value)
    {
        Value = value;
    }
    public override string ToString() => Value;
    #endregion

    // var bookId1 = new BookId("ABC");
    // var bookId2 = new BookId("ABC");
    // bookId1 == bookId2 => true;

    #region Static Method
    public static BookId Create()
    {
        var guid = Guid.NewGuid();
        return new BookId($"{guid}");
    }
    public static BookId Parse(string value)
    {
        if (!Guid.TryParse(value, out var guid))
            throw new InvalidBookIdException(value);
        return new BookId($"{guid}");
    }
    public static bool TryParse(string value, [NotNullWhen(true)] out BookId? bookId)
    {
        if (!Guid.TryParse(value, out var guid))
        {
            bookId = null;
            return false;
        }
        bookId = new BookId($"{guid}");
        return true;
    }
    #endregion
}
