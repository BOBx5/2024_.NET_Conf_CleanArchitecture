namespace LibrarySolution.Application.Abstractions.Commands;

/// <summary>
/// 별도의 결과 Response 없이 <see cref="MediatR.Unit"/>(void)를 반환하는 Command <br/>
/// </summary>
public interface ICommand : MediatR.IRequest<MediatR.Unit>
{

}

/// <summary>
/// <typeparamref name="TResponse"/>를 반환하는 Command
/// </summary>
/// <typeparam name="TResponse"></typeparam>
public interface ICommand<out TResponse> : MediatR.IRequest<TResponse>
{

}
