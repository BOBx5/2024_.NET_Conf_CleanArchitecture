using LibrarySolution.Domain.Aggregates.Books.Entities;
using LibrarySolution.Domain.Aggregates.Rents.Entities;
using LibrarySolution.Domain.Aggregates.Users.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace LibrarySolution.Application.Interfaces;
public interface IApplicationDbContext
{
    DbSet<Book> Books { get; set; }
    DbSet<User> Users { get; set; }
    DbSet<Rent> Rents { get; set; }
    DatabaseFacade Database { get; }
}
