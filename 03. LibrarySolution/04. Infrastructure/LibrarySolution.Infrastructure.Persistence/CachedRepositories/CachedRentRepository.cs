using LibrarySolution.Application.Interfaces;
using LibrarySolution.Domain.Aggregates.Books.ValueObjects;
using LibrarySolution.Domain.Aggregates.Rents.Entities;
using LibrarySolution.Domain.Aggregates.Rents.ValueObjects;
using LibrarySolution.Domain.Aggregates.Users.ValueObjects;
using LibrarySolution.Infrastructure.Persistence.Repositories;
using Microsoft.Extensions.Caching.Memory;

namespace LibrarySolution.Infrastructure.Persistence.CachedRepositories;
internal sealed class CachedRentRepository : RentRepository
{
    private static TimeSpan DefaultSlidingExpiration => TimeSpan.FromMinutes(1);

    #region Constructor
    private readonly IMemoryCache _memoryCache;
    public CachedRentRepository(
        IApplicationDbContext context,
        IMemoryCache memoryCache) : base(context)
    {
        _memoryCache = memoryCache;
    }
    #endregion

    private string GetKeyById(RentId id) => $"Rents::Id={id}";

    #region Query
    public override async Task<List<Rent>> GetRents(UserId? userId = null, BookId? bookId = null, CancellationToken cancellationToken = default)
    {
        var rents = await base.GetRents(userId, bookId, cancellationToken);
        foreach (var rent in rents)
        {
            string keyById = GetKeyById(rent.Id);
            _memoryCache.Set(keyById, rent, DefaultSlidingExpiration);
        }
        return rents;
    }
    public override async Task<Rent?> GetRent(RentId id, CancellationToken cancellationToken = default)
    {
        string keyById = GetKeyById(id);
        if (_memoryCache.TryGetValue(keyById, out Rent? rent) && rent is not null)
            return rent;
        rent = await base.GetRent(id, cancellationToken);
        if (rent is not null)
            _memoryCache.Set(keyById, rent, DefaultSlidingExpiration);
        return rent;
    }
    #endregion

    #region Command
    public override async Task AddRent(Rent rent, CancellationToken cancellationToken = default)
    {
        string keyById = GetKeyById(rent.Id);
        await base.AddRent(rent);
        _memoryCache.Set(keyById, rent, DefaultSlidingExpiration);
    }
    public override async Task Update(Rent rent, CancellationToken cancellationToken = default)
    {
        string keyById = GetKeyById(rent.Id);
        await base.Update(rent);
        _memoryCache.Set(keyById, rent, DefaultSlidingExpiration);
    }
    public override async Task Remove(Rent rent, CancellationToken cancellationToken = default)
    {
        string keyById = GetKeyById(rent.Id);
        await base.Remove(rent);
        _memoryCache.Remove(keyById);
    }
    #endregion
}
