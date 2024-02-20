using LibrarySolution.Application.Abstractions.Queries;
using LibrarySolution.Shared.Extensions;
using MediatR;
using Microsoft.Extensions.Logging;

namespace LibrarySolution.Application.Behaviors;
internal class LoggingQueryBehavior<TQuery, TResponse> : IPipelineBehavior<TQuery, TResponse>
    where TQuery : IQuery<TResponse>
{
    #region Constructor
    private readonly ILogger<LoggingQueryBehavior<TQuery, TResponse>> _logger;
    public LoggingQueryBehavior(ILogger<LoggingQueryBehavior<TQuery, TResponse>> logger)
    {
        _logger = logger;
    }
    #endregion

    #region Handle
    public async Task<TResponse> Handle(TQuery query, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var queryName = query.GetGenericTypeName();
        try
        {
            _logger.LogInformation("Query '{queryName}' requested. \n{@query}", queryName, query);
            var response = await next();
            _logger.LogInformation("Query '{queryName}' responded. \n{@response}", queryName, response);
            return response;
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Query '{queryName}' has failed. \n{@exception}", queryName, ex);
            throw;
        }
    }
    #endregion
}
