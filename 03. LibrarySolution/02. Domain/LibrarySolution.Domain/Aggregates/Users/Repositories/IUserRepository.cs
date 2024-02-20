using LibrarySolution.Domain.Aggregates.Users.Entities;
using LibrarySolution.Domain.Aggregates.Users.ValueObjects;

namespace LibrarySolution.Domain.Aggregates.Users.Repositories;

public interface IUserRepository
{
    Task<List<User>> GetUsers(string? name = null, string? title = null, CancellationToken cancellationToken = default);
    Task<User?> GetUser(UserId id, CancellationToken cancellationToken = default);
    Task AddUser(User user, CancellationToken cancellationToken = default);
    Task Update(User user, CancellationToken cancellationToken = default);
    Task Remove(User user, CancellationToken cancellationToken = default);
}
