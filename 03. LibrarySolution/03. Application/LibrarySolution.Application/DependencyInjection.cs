using FluentValidation;
using MediatR;
using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using LibrarySolution.Application.Behaviors;
using LibrarySolution.Application.Attributes;
using LibrarySolution.Application.Abstractions.Commands;
using LibrarySolution.Application.Abstractions.Queries;

namespace LibrarySolution.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddMediatR(config =>
        {
            config.RegisterServicesFromAssemblyContaining<ApplicationAssembly>();
            config.AddOpenBehavior(typeof(LoggingQueryBehavior<,>)); // IQueryable<T> 로직 로깅
            config.AddOpenBehavior(typeof(LoggingCommandBehavior<,>)); // ICommand<T> 로직 로깅
            config.AddOpenBehavior(typeof(ValidationPipelineBehavior<,>)); // IRequest Validation 파이프라인
            //config.AddOpenBehavior(typeof(IdempotentCommandPipelineBehavior<,>)); // Idempotent Command 처리
        });
        services.AddValidatorsFromAssembly(ApplicationAssembly.Assembly, includeInternalTypes: true);
        return services;
    }

    #region Services.AddValidatorsFromAssembly 수동 구현
    /// <summary>
    /// Services.AddValidatorsFromAssembly 수동 구현
    /// </summary>
    private static IServiceCollection RegisterRequestValidatorFromAssembly(
        this IServiceCollection services,
        Assembly assembly,
        bool includeInternalTypes = true)
    {
        var assemblyTypes = includeInternalTypes ? assembly.GetTypes() : assembly.GetExportedTypes();

        var requestTypes = assemblyTypes
            .Where(type => type.GetInterfaces()
                .Any(x => x.IsGenericType &&
                    (x.GetGenericTypeDefinition() == typeof(ICommand<>) ||
                     x.GetGenericTypeDefinition() == typeof(IQuery<>))))
            .ToList();

        var requestValidatorPairs = requestTypes
            .Select(requestType => new
            {
                RequestType = requestType,
                ValidatorTypes =
                    requestType.GetCustomAttributes<TargetValidatorAttribute>()
                    .Select(attribute => attribute.TargetValidatorType)
            })
            .Where(requestType => requestType.ValidatorTypes.Any());

        foreach (var pair in requestValidatorPairs)
        {
            services.RegisterValidator(pair.RequestType, pair.ValidatorTypes);
        }
        return services;
    }
    private static IServiceCollection RegisterValidator(this IServiceCollection services,
        Type requestType,
        IEnumerable<Type> validatorTypes)
    {
        // add validatorTypes as enumerable
        // example: ServiceProvider.GetService<IValidator<TRequest>>()
        var validatorType = typeof(IValidator<>).MakeGenericType(requestType); // IValidator<TRequest>
        foreach (var validator in validatorTypes)
        {
            services.AddScoped(validatorType, validator);
        }
        return services;
    } 
    #endregion
}