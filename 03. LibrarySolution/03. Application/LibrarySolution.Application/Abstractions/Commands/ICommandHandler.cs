namespace LibrarySolution.Application.Abstractions.Commands;

/// <summary>
/// <typeparamref name="TCommand"/>를 처리하여 <see cref="MediatR.Unit"/>(void)를 반환하는 CommandHandler
/// </summary>
/// <typeparam name="TCommand"><see cref="ICommand"/> 타입의 Generic class</typeparam>
public interface ICommandHandler<TCommand>
    : MediatR.IRequestHandler<TCommand, MediatR.Unit> where TCommand : ICommand
{

}

/// <summary>
/// <typeparamref name="TCommand"/>를 처리하여 <typeparamref name="TResponse"/>를 반환하는 CommandHandler
/// </summary>
/// <typeparam name="TCommand"><see cref="ICommand{TResult}"/> 타입의 Generic class</typeparam>
/// <typeparam name="TResponse"><see cref="ICommand{TResult}"/>이 반환하는 객체 타입</typeparam>
public interface ICommandHandler<in TCommand, TResponse>
    : MediatR.IRequestHandler<TCommand, TResponse> where TCommand : ICommand<TResponse>
{

}
