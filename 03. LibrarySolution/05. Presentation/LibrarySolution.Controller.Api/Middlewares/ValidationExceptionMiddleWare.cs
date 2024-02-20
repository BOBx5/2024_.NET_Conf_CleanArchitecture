using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;

namespace LibrarySolution.Controller.Api.Middlewares;

public class ValidationExceptionMiddleware
{
    private readonly RequestDelegate _next;
    public ValidationExceptionMiddleware(RequestDelegate next)
    {
        _next = next ?? throw new ArgumentNullException(nameof(next));
    }
    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (ValidationException ex)
        {
            if (context.Response.HasStarted)
                throw;

            // HTTP 표준 ProblemDetails 스펙을 따르는 응답을 반환합니다. (https://datatracker.ietf.org/doc/html/rfc7807")
            ProblemDetails problemDetails = new()
            {
                Type = "https://datatracker.ietf.org/doc/html/rfc7807", // 에러 관련 자체 도큐먼트가 있는 경우 URL로 변경
                Title = "Validation error",
                Detail = "One or more validation errors has occurred",
                Status = StatusCodes.Status400BadRequest,
                Instance = context.Request.Path,
            };
            problemDetails.Extensions["traceId"] = context.TraceIdentifier;
            if (ex.Errors is not null)
            {
                problemDetails.Extensions["invalid-params"] = ex.Errors
                    .GroupBy(failure => failure.PropertyName,
                             failure => failure.ErrorMessage,
                    (propertyName, errorMessages) => new
                    {
                        Key = propertyName,
                        Values = errorMessages.Distinct().ToArray()
                    })
                    .ToDictionary(x => x.Key, x => x.Values);
            }
            context.Response.StatusCode = problemDetails.Status.Value;
            context.Response.ContentType = MediaTypeNames.Application.ProblemJson;
            await context.Response.WriteAsJsonAsync(problemDetails);
        }
    }
}
public static class ValidationExceptionMiddlewareExtensions
{
    /// <summary>
    /// <see cref="FluentValidation.ValidationException"/> 발생 시 400 에러로 반환하는 미들웨어를 사용합니다.
    /// </summary>
    public static IApplicationBuilder UseValidationExceptionMiddleware(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<ValidationExceptionMiddleware>();
    }
}