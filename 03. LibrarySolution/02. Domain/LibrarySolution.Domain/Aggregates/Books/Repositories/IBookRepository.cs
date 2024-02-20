using LibrarySolution.Domain.Aggregates.Books.Entities;
using LibrarySolution.Domain.Aggregates.Books.ValueObjects;

namespace LibrarySolution.Domain.Aggregates.Books.Repositories;

public interface IBookRepository
{
    Task<List<Book>> GetBooks(string? title = null, string? author = null, CancellationToken cancellationToken = default);
    Task<Book?> GetBook(BookId id, CancellationToken cancellationToken = default);
    Task Add(Book book, CancellationToken cancellationToken = default);
    Task Update(Book book, CancellationToken cancellationToken = default);
    Task Remove(Book book, CancellationToken cancellationToken = default);
}
