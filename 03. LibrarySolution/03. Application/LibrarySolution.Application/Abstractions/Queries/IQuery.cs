namespace LibrarySolution.Application.Abstractions.Queries;

/// <summary>
/// <typeparamref name="TResponse"/>를 반환하는 Query
/// </summary>
/// <typeparam name="TResponse"></typeparam>
public interface IQuery<out TResponse> : MediatR.IRequest<TResponse>
{

}
