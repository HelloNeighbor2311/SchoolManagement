using Microsoft.AspNetCore.Diagnostics;
using Microsoft.EntityFrameworkCore;
using SchoolManagement.Exceptions;
using SchoolManagement.Models;
using System.Net;
using System.Text.Json;

namespace SchoolManagement.Middleware
{
    public class GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger) : IExceptionHandler
    {
        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            logger.LogError(exception, "An error occured {Message}", exception.Message);
            var errorResponse = CreateErrorResponse(httpContext, exception);
            httpContext.Response.ContentType = "application/json";
            httpContext.Response.StatusCode = errorResponse.StatusCode;

            var json = JsonSerializer.Serialize(errorResponse, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true
            });
            await httpContext.Response.WriteAsync(json, cancellationToken);
            return true;
        }
        private ErrorResponse CreateErrorResponse(HttpContext context, Exception exception)
        {
            var response = new ErrorResponse { TraceId = context.TraceIdentifier };
            switch (exception)
            {
                case NotFoundException notFoundException:
                    response.StatusCode = (int)HttpStatusCode.NotFound;
                    response.Message = "Resource was not found";
                    response.Detail = notFoundException.Message;
                    break;
                case BadRequestException badRequestException:
                    response.StatusCode = (int)HttpStatusCode.BadRequest;
                    response.Message = "Bad request";
                    response.Detail = badRequestException.Message;
                    break;
                case ValidationException validationException:
                    response.StatusCode = (int)HttpStatusCode.UnprocessableEntity;
                    response.Message = "Validation was failed";
                    response.Detail = validationException.Message;
                    break;
                case ConflictException conflictException:
                    response.StatusCode = (int)HttpStatusCode.Conflict;
                    response.Message = "There was a conflict";
                    response.Detail = conflictException.Message;
                    break;
                case DbUpdateConcurrencyException:
                    response.StatusCode = (int)HttpStatusCode.Conflict;
                    response.Message = "Concurrency conflict";
                    response.Detail = "Data has been changed. Please try again";
                    break;
                case ForbiddenException forbiddenException:
                    response.StatusCode = (int)HttpStatusCode.Forbidden;
                    response.Message = "Permission denied";
                    response.Detail = forbiddenException.Message;
                    break;
                default:
                    response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    response.Message = "Internal server error";
                    response.Detail = "An unexpected errors occured. Please try again later";
                    if (context.RequestServices.GetRequiredService<IWebHostEnvironment>().IsDevelopment()) {
                        response.Detail = GetFullExceptionDetail(exception);
                    }
                    break;
            }
            return response;
        }
        private static string GetFullExceptionDetail(Exception exception)
        {
            var messages = new List<string>();
            var current = exception;

            while (current != null)
            {
                // Gom tất cả message trong chuỗi exception
                messages.Add($"[{current.GetType().Name}] {current.Message}");
                current = current.InnerException;
            }

            // Nối lại thành 1 chuỗi để dễ đọc
            return string.Join(" → ", messages);
        }
    }
}
