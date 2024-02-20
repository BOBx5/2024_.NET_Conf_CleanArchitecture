using LibrarySolution.Application.Interfaces;
using LibrarySolution.Domain.Aggregates.Users.Entities;
using LibrarySolution.Domain.Aggregates.Users.Exceptions;
using LibrarySolution.Domain.Aggregates.Users.Repositories;
using LibrarySolution.Domain.Aggregates.Users.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace LibrarySolution.Infrastructure.Persistence.Repositories;
internal class UserRepository : IUserRepository
{
    #region Constructor
    private readonly DbSet<User> _users;
    public UserRepository(IApplicationDbContext context)
    {
        _users = context.Users;
    }
    #endregion

    #region Query
    public virtual async Task<List<User>> GetUsers(string? name = null, string? email = null, CancellationToken cancellationToken = default)
    {
        var query = _users.AsQueryable();
        if (name != null)
            query = query.Where(user => user.Name.Contains(name));
        if (email != null)
            query = query.Where(user => user.Email != null && user.Email.Contains(email));
        return await query.ToListAsync(cancellationToken);
    }
    public virtual async Task<User?> GetUser(UserId id, CancellationToken cancellationToken = default)
    {
        return await _users.FirstOrDefaultAsync(user => user.Id == id, cancellationToken);
    }
    #endregion

    #region Command
    public virtual async Task AddUser(User user, CancellationToken cancellationToken = default)
    {
        await _users.AddAsync(user, cancellationToken);
    }

    public virtual Task Update(User user, CancellationToken cancellationToken = default)
    {
        var entry = _users.Attach(user);
        entry.State = EntityState.Modified;
        return Task.CompletedTask;
    }

    public virtual Task Remove(User user, CancellationToken cancellationToken = default)
    {
        var entry = _users.Attach(user);
        entry.State = EntityState.Deleted;
        return Task.CompletedTask;
    }
    #endregion

}
