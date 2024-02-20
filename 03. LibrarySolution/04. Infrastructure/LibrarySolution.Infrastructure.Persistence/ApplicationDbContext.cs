using LibrarySolution.Application.Interfaces;
using LibrarySolution.Domain.Aggregates.Books.Entities;
using LibrarySolution.Domain.Aggregates.Rents.Entities;
using LibrarySolution.Domain.Aggregates.Users.Entities;
using LibrarySolution.Domain.Primitives;
using LibrarySolution.Infrastructure.Persistence.Outbox;
using LibrarySolution.Infrastructure.Persistence.Interceptors;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Data;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace LibrarySolution.Infrastructure.Persistence;
internal sealed class ApplicationDbContext : DbContext, IApplicationDbContext, IUnitOfWork
{
    #region EF Constructor
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    public ApplicationDbContext(IDateTimeProvider dateTimeProvider)
    {
        _dateTimeProvider = dateTimeProvider;
    }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    #endregion

    #region Tables
    public DbSet<Book> Books { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Rent> Rents { get; set; }
    public DbSet<OutboxMessage> OutboxMessages { get; set; }
    #endregion

    #region Constructor
    private readonly IPublisher _publisher;
    private readonly IDateTimeProvider _dateTimeProvider;
    public ApplicationDbContext(
        DbContextOptions<ApplicationDbContext> options,
        IPublisher publisher,
        IDateTimeProvider dateTimeProvider)
        : base(options)
    {
        _publisher = publisher;
        _dateTimeProvider = dateTimeProvider;
    }
    #endregion

    #region OnModelCreating
    /// <summary>
    /// Persistence의 어셈블리에 <see cref="Microsoft.EntityFrameworkCore.IEntityTypeConfiguration{T}"/>를
    /// 상속받아 FluentApi 이용해 정의된 Entity를 맵핑합니다.
    /// </summary>
    protected override void OnModelCreating(ModelBuilder modelBuilder)  
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
    }
    #endregion

    #region SaveChangesAsync
    /// <inheritdoc cref="IUnitOfWork.SaveChangesAsync(CancellationToken)"/>
    /// <inheritdoc/>
    
    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        // 0. AuditEntityBase 를 가진 Entity의 변경사항을 추적하여 Update/Create 시간을 변경합니다.
        AuditEntities(this.ChangeTracker);

        // 1. 도메인에서 발생한 도메인 이벤트들을 불러옵니다.
        List<DomainEvent> domainEvents = GetDomainEvents(this.ChangeTracker);

        // 2. 데이터베이스에 변경사항이 발생한 Entity들을 저장합니다.
        int result = await base.SaveChangesAsync(cancellationToken);

        // 3. 도메인 이벤트들을 발행하여 도메인이벤트 핸들러들이 처리하도록 합니다.
        foreach (var domainEvent in domainEvents)
        {
            await _publisher.Publish(domainEvent, cancellationToken);
        }
        return result;
    }

    /// <summary>AuditEntityBase 를 가진 Entity의 변경사항을 추적하여 Update/Create 시간을 변경합니다.</summary>
    private void AuditEntities(ChangeTracker changeTracker)
    {
        var auditingEntries = changeTracker.Entries<AuditEntityBase>().ToList();
        foreach (var entry in auditingEntries)
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.SetCreated(_dateTimeProvider.UtcNow);
                    break;
                case EntityState.Modified:
                    entry.Entity.SetUpdated(_dateTimeProvider.UtcNow);
                    break;
            }
        }
    }

    /// <summary>Entitiy들에서 발생한 DomainEvent 목록을 가져옵니다.</summary>
    private List<DomainEvent> GetDomainEvents(ChangeTracker changeTracker)
    {
        return changeTracker.Entries<EntityBase>()
            .Select(e => e.Entity)
            .Where(e => e.GetDomainEvents().Count != 0)
            .SelectMany(e =>
            {
                List<DomainEvent> domainEvents = e.GetDomainEvents().ToList(); 
                e.ClearDomainEvents();
                return domainEvents;
            })
            .ToList();
    }
    #endregion
}