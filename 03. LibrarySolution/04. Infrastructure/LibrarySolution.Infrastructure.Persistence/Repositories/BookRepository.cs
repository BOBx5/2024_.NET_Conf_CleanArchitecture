using LibrarySolution.Application.Interfaces;
using LibrarySolution.Domain.Aggregates.Books.Entities;
using LibrarySolution.Domain.Aggregates.Books.Repositories;
using LibrarySolution.Domain.Aggregates.Books.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace LibrarySolution.Infrastructure.Persistence.Repositories;
internal class BookRepository : IBookRepository
{
    #region Constructor
    private readonly DbSet<Book> _books;
    public BookRepository(IApplicationDbContext context)
    {
        _books = context.Books;
    }
    #endregion

    #region Query
    public virtual async Task<List<Book>> GetBooks(string? title = null, string? author = null, CancellationToken cancellationToken = default)
    {
        var query = _books.AsQueryable();
        if (!string.IsNullOrEmpty(title))
            query = query.Where(book => book.Title.Contains(title));
        if (!string.IsNullOrEmpty(author))
            query = query.Where(book => book.Author.Contains(author));

        return await query.ToListAsync(cancellationToken);
    }
    public virtual async Task<Book?> GetBook(BookId id, CancellationToken cancellationToken = default)
    {
        return await _books.FirstOrDefaultAsync(book => book.Id == id, cancellationToken);
    }
    #endregion

    #region Command
    public virtual async Task Add(Book book, CancellationToken cancellationToken = default)
    {
        await _books.AddAsync(book, cancellationToken);
    }

    public virtual Task Update(Book book, CancellationToken cancellationToken = default)
    {
        var entry = _books.Attach(book);
        entry.State = EntityState.Modified;
        return Task.CompletedTask;
    }
    public virtual Task Remove(Book book, CancellationToken cancellationToken = default)
    {
        var entry = _books.Remove(book);
        entry.State = EntityState.Deleted;
        return Task.CompletedTask;
    }
    #endregion
}
