using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using System;

namespace LibrarySolution.Application.Behaviors;

public class ValidationPipelineBehavior<TRequest, TResponse>
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
    where TResponse : notnull
{
    #region Constructor
    private readonly ILogger<ValidationPipelineBehavior<TRequest, TResponse>> _logger;
    private readonly IEnumerable<IValidator<TRequest>> _validators;
    public ValidationPipelineBehavior(
        ILogger<ValidationPipelineBehavior<TRequest, TResponse>> logger,
        IEnumerable<IValidator<TRequest>> validators)
    {
        _validators = validators;
        _logger = logger;
    }
    #endregion

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        var requestTypeName = request.GetGenericTypeName();
        if (!_validators.Any())
        {
            _logger.LogInformation("No validators found for {RequestTypeName}", requestTypeName);
            return await next();
        }

        var context = new ValidationContext<TRequest>(request);

        _logger.LogInformation("Validating request {RequestTypeName}", requestTypeName);
        var errors = _validators
             .Select(validator => validator.Validate(context))
             .SelectMany(validationResult => validationResult.Errors)
             .Where(failure => failure is not null);

        if (errors.Any())
        {
            throw new ValidationException(errors);
        }

        return await next();
    }
}
