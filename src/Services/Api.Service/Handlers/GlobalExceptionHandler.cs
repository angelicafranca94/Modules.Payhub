using Api.Pix.Application.Interfaces.Repositories;
using Api.Pix.Domain.Models;
using CrossCutting.PayHub.Shared.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Polly.CircuitBreaker;
using System.Net;

namespace Api.Service.Handlers;

internal sealed class GlobalExceptionHandler : IExceptionHandler
{
    private readonly ILogger<GlobalExceptionHandler> _logger;
    private readonly IServiceProvider _serviceProvider;
    public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger, IServiceProvider serviceProvider)
    {
        _logger = logger;
        _serviceProvider = serviceProvider;
    }

    public async ValueTask<bool> TryHandleAsync(
       HttpContext httpContext,
       Exception exception,
       CancellationToken cancellationToken)
    {
        ProblemDetails result;
        var messageErrorDetail = string.Empty;

        using (var scope = _serviceProvider.CreateScope())
        {
            switch (exception)
            {
                case BadRequestException badRequestException:
                    messageErrorDetail = badRequestException.Message;
                    result = await GenerateResultErrorObject(messageErrorDetail, badRequestException, httpContext, HttpStatusCode.BadRequest, scope);
                    break;

                case NotFoundException notFoundException:
                    messageErrorDetail = notFoundException.Message;
                    result = await GenerateResultErrorObject(messageErrorDetail, notFoundException, httpContext, HttpStatusCode.NotFound, scope);
                    break;

                case BrokenCircuitException brokenCircuitException:
                    messageErrorDetail = "The service is temporarily unavailable.";
                    result = await GenerateResultErrorObject(messageErrorDetail, brokenCircuitException, httpContext, HttpStatusCode.NotFound, scope);
                    break;

                default:
                    messageErrorDetail = exception.Message;
                    result = await GenerateResultErrorObject(messageErrorDetail, exception, httpContext, HttpStatusCode.InternalServerError, scope);
                    break;
            }

            _logger.LogError(exception, "Exception occurred: {Message}", exception.Message);

            await httpContext.Response.WriteAsJsonAsync(result, cancellationToken: cancellationToken);
            return true;
        }
    }

    private async Task<ProblemDetails> GenerateResultErrorObject(string exceptionDetail, Exception exceptionCustom, HttpContext httpContext, HttpStatusCode statusCode, IServiceScope scope)
    {
        var problemDetails = new ProblemDetails
        {
            Status = (int)statusCode,
            Type = exceptionCustom.GetType().Name,
            Title = "An unexpected error occurred",
            Detail = exceptionDetail,
            Instance = $"{httpContext.Request.Method} - {httpContext.Request.Path}",
        };

        var pixError = new PixControlLogErrorModel
        {
            ErrorMessage = $"{exceptionCustom} - {problemDetails.Instance}",
            StackTrace = exceptionCustom.StackTrace,

        };

        var logErrorRepository = scope.ServiceProvider.GetRequiredService<IPixControlLogErrorRepository>();
        await logErrorRepository.InsertAsync(pixError);

        return problemDetails;
    }
}
