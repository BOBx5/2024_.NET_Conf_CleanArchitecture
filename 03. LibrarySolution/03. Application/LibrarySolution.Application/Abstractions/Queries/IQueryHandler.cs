using MediatR;

namespace LibrarySolution.Application.Abstractions.Queries;

/// <summary>
/// <typeparamref name="TQuery"/>를 처리하여 <typeparamref name="TResponse"/>를 반환하는 QueryHandler
/// </summary>
/// <typeparam name="TQuery"><see cref="IQuery{TResult}"/> 타입의 Query Generic</typeparam>
/// <typeparam name="TResponse"><see cref="IQuery{TResult}"/>가 반환하는 객체 Type</typeparam>
public interface IQueryHandler<TQuery, TResponse>
    : IRequestHandler<TQuery, TResponse> where TQuery : IQuery<TResponse>
{

}