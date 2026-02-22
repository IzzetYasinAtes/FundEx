namespace FundEx.WebApi.Middlewares;
using System.Net;
using System.Text.Json;
using FundEx.Application.Common.Exceptions;
using FundEx.Domain.Common;
using FluentValidation;

public class GlobalExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionMiddleware> _logger;

    public GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unhandled exception: {Message}", ex.Message);
            await HandleExceptionAsync(context, ex);
        }
    }

    private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/problem+json";

        var (statusCode, title, detail) = exception switch
        {
            ValidationException validationEx => (
                (int)HttpStatusCode.BadRequest,
                "Validation Error",
                JsonSerializer.Serialize(validationEx.Errors.Select(e => new { e.PropertyName, e.ErrorMessage }))
            ),
            NotFoundException notFoundEx => (
                (int)HttpStatusCode.NotFound,
                "Not Found",
                notFoundEx.Message
            ),
            BusinessRuleValidationException ruleEx => (
                (int)HttpStatusCode.UnprocessableEntity,
                "Business Rule Violation",
                ruleEx.Details
            ),
            _ => (
                (int)HttpStatusCode.InternalServerError,
                "Internal Server Error",
                "An unexpected error occurred."
            )
        };

        context.Response.StatusCode = statusCode;
        var problemDetails = new
        {
            type = $"https://httpstatuses.com/{statusCode}",
            title,
            status = statusCode,
            detail,
            instance = context.Request.Path.Value
        };

        await context.Response.WriteAsJsonAsync(problemDetails);
    }
}
