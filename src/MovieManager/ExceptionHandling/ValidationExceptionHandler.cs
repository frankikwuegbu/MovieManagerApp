using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace API.ExceptionHandling;

internal sealed class ValidationExceptionHandler(IProblemDetailsService problemDetailsService,
    ILogger<ValidationExceptionHandler> logger) : IExceptionHandler
{
    private readonly IProblemDetailsService problemDetailsService = problemDetailsService;
    private readonly ILogger<ValidationExceptionHandler> logger = logger;

    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, 
        Exception exception,
        CancellationToken cancellationToken)
    {
        if (exception is not ValidationException validationException)
        {
            return false;
        }

        logger.LogWarning($"a validation exception occured: {validationException}",
            validationException.Errors);

        httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
        var context = new ProblemDetailsContext
        {
            HttpContext = httpContext,
            Exception = validationException,
            ProblemDetails = new ProblemDetails
            {
                Title = "one or more validation errors occured",
                Detail = exception.Message,
                Status = StatusCodes.Status400BadRequest
            }
        };

        var errors = validationException.Errors
                .GroupBy(e => e.PropertyName)
                .ToDictionary(
                g => g.Key.ToLowerInvariant(),
                g => g.Select(e => e.ErrorMessage).ToArray()
            );
        context.ProblemDetails.Extensions.Add("errors", errors);

        return await problemDetailsService.TryWriteAsync(context);
    }
}