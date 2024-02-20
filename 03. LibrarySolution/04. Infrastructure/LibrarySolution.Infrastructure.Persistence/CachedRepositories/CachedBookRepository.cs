using LibrarySolution.Application.Interfaces;
using LibrarySolution.Domain.Aggregates.Books.Entities;
using LibrarySolution.Domain.Aggregates.Books.Exceptions;
using LibrarySolution.Domain.Aggregates.Books.ValueObjects;
using LibrarySolution.Infrastructure.Persistence.Repositories;
using Microsoft.Extensions.Caching.Memory;

namespace LibrarySolution.Infrastructure.Persistence.CachedRepositories
{
    internal sealed class CachedBookRepository : BookRepository
    {
        private static TimeSpan DefaultSlidingExpiration => TimeSpan.FromMinutes(1);
        private static string GetKeyById(BookId id) => $"Books::Id={id}";

        #region Constructor
        private readonly IMemoryCache _memoryCache;
        public CachedBookRepository(
            IApplicationDbContext context,
            IMemoryCache memoryCache) : base(context)
        {
            _memoryCache = memoryCache;
        }
        #endregion


        #region Query
        public override async Task<List<Book>> GetBooks(string? title = null, string? author = null, CancellationToken cancellationToken = default)
        {
            var books = await base.GetBooks(title, author, cancellationToken);
            foreach (var book in books)
            {
                string keyById = GetKeyById(book.Id);
                _memoryCache.Set(keyById, book, DefaultSlidingExpiration);
            }
            return books;
        }
        public override async Task<Book?> GetBook(BookId id, CancellationToken cancellationToken = default)
        {
            string keyById = GetKeyById(id); // Books::Id=0000000-00000....
            if (_memoryCache.TryGetValue(keyById, out Book? book) && book is not null)
                return book;
            
            book = await base.GetBook(id, cancellationToken);
            if (book is not null)
                _memoryCache.Set(keyById, book, DefaultSlidingExpiration);
            return book;
        }
        #endregion

        #region Command
        public override async Task Add(Book book, CancellationToken cancellationToken = default)
        {
            string keyById = GetKeyById(book.Id);
            await base.Add(book, cancellationToken);
            _memoryCache.Set(keyById, book, DefaultSlidingExpiration);
        }

        public override async Task Remove(Book book, CancellationToken cancellationToken = default)
        {
            string keyById = GetKeyById(book.Id);
            await base.Remove(book, cancellationToken);
            _memoryCache.Remove(keyById);
        }

        public override async Task Update(Book book, CancellationToken cancellationToken = default)
        {
            string keyById = GetKeyById(book.Id);
            await base.Update(book, cancellationToken);
            _memoryCache.Set(keyById, book, DefaultSlidingExpiration);
        }
        #endregion
    }
}
