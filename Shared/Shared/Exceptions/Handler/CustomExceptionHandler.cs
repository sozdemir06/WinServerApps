using System.Net;
using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Shared.Exceptions.Handler;

/// <summary>
/// Custom exception handler for handling application-specific exceptions
/// </summary>
public class CustomExceptionHandler() : IExceptionHandler
{


  public async ValueTask<bool> TryHandleAsync(
      HttpContext context,
      Exception exception,
      CancellationToken cancellationToken)
  {

    (string Detail, string Title, int StatusCode) details = exception switch
    {
      InternalServerErrorException =>
      (
          exception.Message,
          exception.GetType().Name,
          context.Response.StatusCode = StatusCodes.Status500InternalServerError
      ),
      ValidationException =>
      (
          exception.Message,
          exception.GetType().Name,
          context.Response.StatusCode = StatusCodes.Status400BadRequest
      ),
      BadRequestException =>
      (
          exception.Message,
          exception.GetType().Name,
          context.Response.StatusCode = StatusCodes.Status400BadRequest
      ),
      NotFoundException =>
      (
          exception.Message,
          exception.GetType().Name,
          context.Response.StatusCode = StatusCodes.Status404NotFound
      ),
      ForbiddenException =>
      (
          exception.Message,
          exception.GetType().Name,
          context.Response.StatusCode = StatusCodes.Status403Forbidden
      ),
      _ =>
      (
          exception.Message,
          exception.GetType().Name,
          context.Response.StatusCode = StatusCodes.Status500InternalServerError
      )
    };

    var problemDetails = new ProblemDetails
    {
      Title = details.Title,
      Detail = details.Detail,
      Status = details.StatusCode,
      Instance = context.Request.Path
    };

    problemDetails.Extensions.Add("traceId", context.TraceIdentifier);

    if (exception is ValidationException validationException)
    {
      problemDetails.Extensions.Add("ValidationErrors", validationException.Errors);
    }

    await context.Response.WriteAsJsonAsync(problemDetails, cancellationToken: cancellationToken);
    return true;
  }
}