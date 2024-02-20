using LibrarySolution.Domain.Primitives;

namespace LibrarySolution.Application.Abstractions.Events
{
    /// <summary>
    /// <typeparamref name="TDomainEvent"/> 도메인 이벤트를 처리하는 핸들러입니다.
    /// </summary>
    /// <typeparam name="TDomainEvent"></typeparam>
    internal interface IDomainEventHandler<in TDomainEvent> : MediatR.INotificationHandler<TDomainEvent>
        where TDomainEvent : DomainEvent
    {

    }
}
