using LibrarySolution.Application.Interfaces;
using LibrarySolution.Domain.Primitives;
using LibrarySolution.Infrastructure.Persistence.Outbox;
using LibrarySolution.Shared.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace LibrarySolution.Infrastructure.Persistence.Interceptors;

/// <summary>
/// <see cref="Microsoft.EntityFrameworkCore.DbContext.SaveChangesAsync(CancellationToken)"/> 호출하는 시점에
/// <see cref="EntityBase"/>에 등록된 <see cref="DomainEvent"/>들을 
/// <see cref="OutboxMessage"/>로 변환하여 저장합니다.
/// </summary>
internal sealed class DomainEventsToOutboxMessageInterceptor : SaveChangesInterceptor
{
    private readonly IDateTimeProvider _dateTimeProvider;
    public DomainEventsToOutboxMessageInterceptor(IDateTimeProvider dateTimeProvider)
    {
        _dateTimeProvider = dateTimeProvider;
    }

    public override async ValueTask<int> SavedChangesAsync(SaveChangesCompletedEventData eventData, int result, CancellationToken cancellationToken = default)
    {
        //return await base.SavedChangesAsync(eventData, result, cancellationToken);

        DbContext? dbContext = eventData.Context;
        if (dbContext is null)
            return await base.SavedChangesAsync(eventData, result, cancellationToken);

        var outboxMessages = dbContext.ChangeTracker
            .Entries<EntityBase>()
            .Select(entry => entry.Entity)
            .Where(e => e.GetDomainEvents().Count != 0)
            .SelectMany(entity =>
            {
                // 반드시 ToList()를 통해 재할당해야합니다. 그렇지 않으면 ClearDomainEvents() 실행 시점에 도메인 이벤트가 삭제됩니다.
                List<DomainEvent> domainEvents = entity.GetDomainEvents().ToList();
                entity.ClearDomainEvents();
                return domainEvents;
            })
            .Select(domainEvent => new OutboxMessage
            {
                Id = Guid.NewGuid(),
                Type = domainEvent.GetGenericTypeName(),
                Content = JsonSerializer.Serialize(domainEvent),
                OccurredOn = _dateTimeProvider.UtcNow
            })
            .ToList();

        await dbContext.Set<OutboxMessage>().AddRangeAsync(outboxMessages, cancellationToken);
        return await base.SavedChangesAsync(eventData, result, cancellationToken);
    }
}
internal static class DomainEventsToOutboxMessageInterceptorExtensions
{
    /// <summary>
    /// <see cref="DomainEventsToOutboxMessageInterceptor"/>를 <see cref="DbContextOptionsBuilder"/>에 추가합니다.
    /// </summary>
    /// <param name="options"></param>
    /// <param name="serviceProvider"></param>
    /// <param name="throwExceptionWhenNotRegistered">
    /// 서비스에 <see cref="DomainEventsToOutboxMessageInterceptor"/>가 등록되지 않은 경우 예외를 발생시킬지 여부를 설정합니다. (기본: false)
    /// </param>
    public static void AddDomainEventsToOutboxMessageInterceptor(
        this DbContextOptionsBuilder options,
        IServiceProvider serviceProvider,
        bool throwExceptionWhenNotRegistered = false)
    {
        DomainEventsToOutboxMessageInterceptor? outboxIntercepter;

        outboxIntercepter  = throwExceptionWhenNotRegistered 
            ? serviceProvider.GetRequiredService<DomainEventsToOutboxMessageInterceptor>()
            : serviceProvider.GetService<DomainEventsToOutboxMessageInterceptor>();

        if (outboxIntercepter is not null)
        {
            options.AddInterceptors(outboxIntercepter);
        }
    }
}