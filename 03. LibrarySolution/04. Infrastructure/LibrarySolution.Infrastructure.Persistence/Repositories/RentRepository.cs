using LibrarySolution.Application.Interfaces;
using LibrarySolution.Domain.Aggregates.Books.ValueObjects;
using LibrarySolution.Domain.Aggregates.Rents.Entities;
using LibrarySolution.Domain.Aggregates.Rents.Repositories;
using LibrarySolution.Domain.Aggregates.Rents.ValueObjects;
using LibrarySolution.Domain.Aggregates.Users.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace LibrarySolution.Infrastructure.Persistence.Repositories;
internal class RentRepository : IRentRepository
{
    #region Constructor
    private readonly DbSet<Rent> _rents;
    public RentRepository(IApplicationDbContext context)
    {
        _rents = context.Rents;
    }
    #endregion

    #region Query
    public virtual async Task<List<Rent>> GetRents(UserId? userId = null, BookId? bookId = null, CancellationToken cancellationToken = default)
    {
        var query = _rents.AsQueryable();
        if (userId != null)
            query = query.Where(rent => rent.UserId == userId);
        if (bookId != null)
            query = query.Where(rent => rent.BookId == bookId);

        return await query.ToListAsync();
    }
    public virtual async Task<Rent?> GetRent(RentId id, CancellationToken cancellationToken = default)
    {
        return await _rents.FirstOrDefaultAsync(rent => rent.Id == id);
    }
    #endregion

    #region Command
    public virtual async Task AddRent(Rent rent, CancellationToken cancellationToken = default)
    {
        await _rents.AddAsync(rent, cancellationToken);
    }
    public virtual Task Update(Rent rent, CancellationToken cancellationToken = default)
    {
        var entry = _rents.Attach(rent);
        entry.State = EntityState.Modified;
        return Task.CompletedTask;
    }
    public virtual Task Remove(Rent rent, CancellationToken cancellationToken = default)
    {
        var entry = _rents.Attach(rent);
        entry.State = EntityState.Deleted;
        return Task.CompletedTask;
    }
    #endregion
}
