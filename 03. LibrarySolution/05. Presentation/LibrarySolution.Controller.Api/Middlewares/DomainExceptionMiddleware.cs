using LibrarySolution.Domain.Primitives;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;

namespace LibrarySolution.Controller.Api.Middlewares;

public class DomainExceptionMiddleware
{
    private readonly RequestDelegate _next;
    public DomainExceptionMiddleware(RequestDelegate next)
    {
        _next = next ?? throw new ArgumentNullException(nameof(next));
    }
    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (DomainException ex)
        {
            if (context.Response.HasStarted)
                throw;

            // HTTP 표준 ProblemDetails 스펙을 따르는 응답을 반환합니다. (https://datatracker.ietf.org/doc/html/rfc7807")
            ProblemDetails problemDetails = new()
            {
                Type = "https://datatracker.ietf.org/doc/html/rfc7807", // 에러 관련 자체 도큐먼트가 있는 경우 URL로 변경
                Title = ex.GetType().Name,
                Detail = ex.Message,
                Status = StatusCodes.Status500InternalServerError,
                Instance = context.Request.Path,
            };
            problemDetails.Extensions["traceId"] = context.TraceIdentifier;
            if (ex.Errors is not null)
            {
                problemDetails.Extensions["errors"] = ex.Errors;
            }
            context.Response.StatusCode = problemDetails.Status.Value;
            context.Response.ContentType = MediaTypeNames.Application.Json;
            await context.Response.WriteAsJsonAsync(problemDetails);
        }
    }
}
public static class DomainExceptionMiddlewareExtensions
{
    /// <summary>
    /// <see cref="DomainException"/> 발생 시 HTTP 표준 ProbelmDetails로 변환하는 미들웨어를 사용합니다.
    /// </summary>
    public static IApplicationBuilder UseDomainExceptionMiddleware(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<DomainExceptionMiddleware>();
    }
}