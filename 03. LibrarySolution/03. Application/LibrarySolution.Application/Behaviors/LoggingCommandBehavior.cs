using LibrarySolution.Application.Abstractions.Commands;
using LibrarySolution.Shared.Extensions;
using MediatR;
using Microsoft.Extensions.Logging;

namespace LibrarySolution.Application.Behaviors;

//internal class LoggingCommandBehavior<TCommand, TResponse> : IPipelineBehavior<TCommand, TResponse>
//    where TCommand : ICommand<TResponse>
//{
//    #region Constructor
//    private readonly ILogger<LoggingCommandBehavior<TCommand, TResponse>> _logger;
//    public LoggingCommandBehavior(ILogger<LoggingCommandBehavior<TCommand, TResponse>> logger)
//    {
//        _logger = logger;
//    }
//    #endregion

//    #region Handle
//    public async Task<TResponse> Handle(TCommand command, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
//    {
//        var commandName = command.GetGenericTypeName();
//        try
//        {
//            _logger.LogInformation("Command '{commandName}' requested. ({@command})", commandName, command);
//            var response = await next();
//            _logger.LogInformation("Command '{commandName}' responded. ({@command}) ({@response})", commandName, command, response);
//            return response;
//        }
//        catch (Exception ex)
//        {
//            _logger.LogWarning(ex, "Command '{commandName}' has failed. ({@exception})", commandName, ex);
//            throw;
//        }
//    }
//    #endregion
//}

internal class LoggingCommandBehavior<TCommand, TResponse> : IPipelineBehavior<TCommand, TResponse>
    where TCommand : ICommand<TResponse>
{
    #region Constructor
    private readonly ILogger<LoggingCommandBehavior<TCommand, TResponse>> _logger;
    public LoggingCommandBehavior(ILogger<LoggingCommandBehavior<TCommand, TResponse>> logger)
    {
        _logger = logger;
    }
    #endregion
    
    #region Handle
    public async Task<TResponse> Handle(TCommand command, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var commandName = command.GetGenericTypeName();
        try
        {
            _logger.LogInformation("Command '{commandName}' requested. \n{@command}", commandName, command);
            var response = await next();
            _logger.LogInformation("Command '{commandName}' responded. \n{@response}", commandName, response);
            return response;
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Command '{commandName}' has failed. \n{@exception}", commandName, ex);
            throw;
        }
    }
    #endregion
}
