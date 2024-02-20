using LibrarySolution.Domain.Aggregates.Books.ValueObjects;
using LibrarySolution.Domain.Aggregates.Rents.DomainEvents;
using LibrarySolution.Domain.Aggregates.Rents.Exceptions;
using LibrarySolution.Domain.Aggregates.Rents.ValueObjects;
using LibrarySolution.Domain.Aggregates.Users.ValueObjects;

namespace LibrarySolution.Domain.Aggregates.Rents.Entities;
public sealed class Rent : AuditEntityBase, IAggregateRoot
{
    #region EF Constructor
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    private Rent() { }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    #endregion

    #region Property
    /// <summary>대여고유ID</summary>
    public RentId Id { get; private set; }

    /// <summary>대여 유저ID</summary>
    public UserId UserId { get; private set; }

    /// <summary>대여 도서ID</summary>
    public BookId BookId { get; private set; }

    /// <summary>대여일시</summary>
    public DateTime BorrowedAt { get; private set; }

    /// <summary>반납기한</summary>
    public DateTime DueDate { get; private set; }

    /// <summary>반납일시</summary>
    public DateTime? ReturnedAt { get; private set; }

    /// <summary>반납여부</summary>
    public bool IsReturned => ReturnedAt.HasValue;
    #endregion

    #region Constructor
    private Rent(RentId id, UserId userId, BookId bookId, DateTime borrowedAt, DateTime dueDate)
    {
        Id = id;
        UserId = userId;
        BookId = bookId;
        BorrowedAt = borrowedAt;
        DueDate = dueDate;
    }
    public static Rent Create(UserId userId, BookId bookId, DateTime borrowedAt)
    {
        var id = RentId.Create();
        var rent = new Rent(id, userId, bookId, borrowedAt, borrowedAt.AddDays(DefaultExtensionDays));
        rent.Raise(new RentCreatedDomainEvent
        {
            RentId = id,
            BookId = bookId,
            UserId = userId,
            BorrowedAt = borrowedAt,
        });
        return rent;
    }
    #endregion

    #region Domain Logic
    const int DefaultExtensionDays = 7;
    public void Extend(int? extendingDays = DefaultExtensionDays)
    {
        if (IsReturned)
            throw new RentAlreadyReturnedException(this.Id);
        this.DueDate = DueDate.AddDays(extendingDays ?? DefaultExtensionDays);
        this.Raise(new RentExtendedDomainEvent
        {
            RentId = this.Id,
            BookId = this.BookId,
            UserId = this.UserId,
            DueDate = this.DueDate
        });
    }
    public void Return(DateTime returnedAt)
    {
        if (IsReturned)
            throw new RentAlreadyReturnedException(this.Id);
        this.ReturnedAt = returnedAt;
        this.Raise(new RentReturnedDomainEvent
        {
            RentId = this.Id,
            BookId = this.BookId,
            UserId = this.UserId,
            ReturnedAt = returnedAt
        });
    }
    #endregion
}
