using LibrarySolution.Domain.Aggregates.Books.ValueObjects;
using LibrarySolution.Domain.Aggregates.Rents.Entities;
using LibrarySolution.Domain.Aggregates.Rents.ValueObjects;
using LibrarySolution.Domain.Aggregates.Users.ValueObjects;

namespace LibrarySolution.Domain.Aggregates.Rents.Repositories;

public interface IRentRepository
{
    Task<List<Rent>> GetRents(UserId? userId = null, BookId? bookId = null, CancellationToken cancellationToken = default);
    Task<Rent?> GetRent(RentId id, CancellationToken cancellationToken = default);
    Task AddRent(Rent rent, CancellationToken cancellationToken = default);
    Task Update(Rent rent, CancellationToken cancellationToken = default);
    Task Remove(Rent rent, CancellationToken cancellationToken = default);
}
