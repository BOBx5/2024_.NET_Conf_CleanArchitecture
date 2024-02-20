using LibrarySolution.Application.Interfaces;
using LibrarySolution.Domain.Aggregates.Users.Entities;
using LibrarySolution.Domain.Aggregates.Users.Exceptions;
using LibrarySolution.Domain.Aggregates.Users.ValueObjects;
using LibrarySolution.Infrastructure.Persistence.Repositories;
using Microsoft.Extensions.Caching.Memory;

namespace LibrarySolution.Infrastructure.Persistence.CachedRepositories
{
    internal sealed class CachedUserRepository : UserRepository
    {
        private static TimeSpan DefaultSlidingExpiration => TimeSpan.FromMinutes(1);

        #region Constructor
        private readonly IMemoryCache _memoryCache;
        public CachedUserRepository(
            IApplicationDbContext context,
            IMemoryCache memoryCache) : base(context)
        {
            _memoryCache = memoryCache;
        }
        #endregion

        private string GetKeyById(UserId id) => $"Users::Id={id}";

        #region Query
        public override async Task<User?> GetUser(UserId id, CancellationToken cancellationToken = default)
        {
            string keyById = GetKeyById(id);
            if (_memoryCache.TryGetValue(keyById, out User? user) && user is not null)
                return user;

            user = await base.GetUser(id, cancellationToken);
            if (user is not null)
                _memoryCache.Set(keyById, user, DefaultSlidingExpiration);
            return user;
        }

        public override async Task<List<User>> GetUsers(string? name = null, string? email = null, CancellationToken cancellationToken = default)
        {
            var users = await base.GetUsers(name, email, cancellationToken);
            foreach (var user in users)
            {
                string keyById = GetKeyById(user.Id);
                _memoryCache.Set(keyById, user, DefaultSlidingExpiration);
            }
            return users;
        }
        #endregion

        #region Command
        public override async Task AddUser(User user, CancellationToken cancellationToken = default)
        {
            await base.AddUser(user, cancellationToken);
            string keyById = GetKeyById(user.Id);
            _memoryCache.Set(keyById, user, DefaultSlidingExpiration);
        }

        public override async Task Update(User user, CancellationToken cancellationToken = default)
        {
            await base.Update(user, cancellationToken);
            string keyById = GetKeyById(user.Id);
            _memoryCache.Set(keyById, user, DefaultSlidingExpiration);
        }

        public override async Task Remove(User user, CancellationToken cancellationToken = default)
        {
            await base.Remove(user, cancellationToken);
            string keyById = GetKeyById(user.Id);
            _memoryCache.Remove(keyById);
        }
        #endregion
    }
}
