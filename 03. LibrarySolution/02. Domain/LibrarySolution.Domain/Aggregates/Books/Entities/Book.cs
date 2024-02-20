using LibrarySolution.Domain.Aggregates.Books.DomainEvents;
using LibrarySolution.Domain.Aggregates.Books.Exceptions;
using LibrarySolution.Domain.Aggregates.Books.ValueObjects;

namespace LibrarySolution.Domain.Aggregates.Books.Entities;

public sealed class Book : AuditEntityBase, IAggregateRoot
{
    #region EF Constructor
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    private Book() { }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    #endregion

    #region Property
    /// <summary>도서고유ID</summary>
    public BookId Id { get; private set; }

    ///<summary>도서 제목</summary>
    public string Title { get; private set; }

    ///<summary>도서 설명</summary>
    public string Description { get; private set; }

    ///<summary>도서 작가</summary>
    public string Author { get; private set; }

    ///<summary>보유 수량</summary>
    public int Quantity { get; private set; }
    #endregion

    #region Constructor
    private Book(BookId id, string title, string description, string author, int quantity)
    {
        Id = id;
        Title = title;
        Description = description;
        Author = author;
        Quantity = quantity;
    }
    public static Book Create(string title, string description, string author, int quantity)
    {
        var newBookId = BookId.Create();
        return new Book(newBookId, title, description, author, quantity);
    }
    #endregion

    #region Domain Logic
    public void ModifyInfo(string? title, string? description, string? author)
    {
        if (title == null && description == null && author == null)
            throw new InvalidBookInfoException("변경할 도서 데이터가 없습니다.");

        if (title != null) Title = title;
        if (description != null) Description = description;
        if (author != null) Author = author;

        this.Raise(new BookInfoModifiedDomainEvent
        {
            BookId = this.Id,
            Title = title,
            Description = description,
            Author = author,
        });
    }

    public void IncreaseQunatity()
    {
        Quantity++;
        this.Raise(new BookQuantityModifiedDomainEvent
        {
            BookId = this.Id,
            ChangedQuantity = this.Quantity
        });
    }

    public void DecreaseQuantity()
    {
        if (Quantity == 0)
            throw new BookOutOfStockException(this.Id);

        Quantity--;
        this.Raise(new BookQuantityModifiedDomainEvent
        {
            BookId = this.Id,
            ChangedQuantity = this.Quantity
        });
    }
    #endregion
}
