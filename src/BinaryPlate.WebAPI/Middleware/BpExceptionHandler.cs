namespace BinaryPlate.WebAPI.Middleware;

/// <summary>
/// Represents an exception handler middleware for BlazorPlate applications.
/// </summary>
public class BpExceptionHandler : IExceptionHandler
{
    #region Public Methods

    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        // Obtain logger factory and app options service from the HttpContext services
        var loggerFactory = httpContext.RequestServices.GetService<ILoggerFactory>();
        var appOptionsService = httpContext.RequestServices.GetService<IAppOptionsService>();

        // Get application exception handling options
        var exceptionOptions = appOptionsService.GetAppExceptionOptions();

        // Create a logger instance based on the logger factory
        var logger = loggerFactory?.CreateLogger("BpExceptionHandler");

        // Log the exception if logging is enabled
        if (exceptionOptions.LoggingEnabled && logger != null)
            logger.LogError(exception, exception.Message);

        // Create an instance of ApiErrorResponse to build the response
        var apiErrorResponse = new ApiErrorResponse
        {
            Instance = httpContext.Request.Path
        };

        // Handle specific exception types
        switch (exception)
        {
            // Handle FluentValidationException
            case FluentValidationException fluentValidationException:
                apiErrorResponse.Type = "https://httpstatuses.com/422";
                apiErrorResponse.Title = Resource.Model_validation_errors_occurred_while_processing_your_request;
                apiErrorResponse.Status = HttpStatusCode.UnprocessableEntity;

                // Map FluentValidation errors to ValidationError
                apiErrorResponse.ValidationErrors = fluentValidationException.Errors.Select(failure => new ValidationError
                {
                    Name = failure.PropertyName,
                    Reason = failure.ErrorMessage
                }).ToList();
                break;

            // Handle ApplicationResultException
            case ApplicationResultException applicationResultException:
                apiErrorResponse.Type = "https://httpstatuses.com/422";
                apiErrorResponse.Title = Resource.Application_errors_occurred_while_processing_your_request;
                apiErrorResponse.Status = HttpStatusCode.UnprocessableEntity;

                // Map application result errors to ValidationError
                apiErrorResponse.ValidationErrors = applicationResultException.ValidationErrors.Select(failure => new ValidationError
                {
                    Name = failure.Key,
                    Reason = failure.Value
                }).ToList();
                break;

            // Handle UnauthorizedAccessException
            case UnauthorizedAccessException unauthorizedAccessException:
                apiErrorResponse.Type = "https://httpstatuses.com/401";
                apiErrorResponse.Title = unauthorizedAccessException.Message;
                apiErrorResponse.Status = HttpStatusCode.Unauthorized;
                break;

            // Handle ForbiddenAccessException
            case ForbiddenAccessException forbiddenAccessException:
                apiErrorResponse.Type = "https://httpstatuses.com/403";
                apiErrorResponse.Title = forbiddenAccessException.Message;
                apiErrorResponse.Status = HttpStatusCode.Forbidden;
                break;

            // Handle TwoFactorAuthRequiredException
            case TwoFactorAuthRequiredException twoFactorAuthRequiredException:
                apiErrorResponse.Type = "https://httpstatuses.com/423";
                apiErrorResponse.Title = twoFactorAuthRequiredException.Message;
                apiErrorResponse.Status = HttpStatusCode.Locked;
                break;

            // Handle TwoFactorAuthRedirectionException
            case TwoFactorAuthRedirectionException twoFactorAuthRedirectionException:
                apiErrorResponse.Type = "https://httpstatuses.com/302";
                apiErrorResponse.Title = twoFactorAuthRedirectionException.Message;
                apiErrorResponse.Status = HttpStatusCode.Redirect;
                break;

            // Handle BadRequestException
            case BadRequestException:
                apiErrorResponse.Type = "https://httpstatuses.com/400";
                apiErrorResponse.Title = Resource.The_request_was_not_processed_due_to_invalid_or_missing_parameters;
                apiErrorResponse.Status = HttpStatusCode.BadRequest;
                apiErrorResponse.ErrorMessage = exception.Message;

                // Include inner exception and stack trace if detailed exceptions are enabled
                apiErrorResponse.InnerException = exceptionOptions.DetailedExceptionEnabled ? exception.InnerException?.ToString() : string.Empty;
                apiErrorResponse.StackTrace = exceptionOptions.DetailedExceptionEnabled ? exception.StackTrace : string.Empty;
                break;

            // Handle NotFoundException
            case NotFoundException:
                apiErrorResponse.Type = "https://httpstatuses.com/404";
                apiErrorResponse.Title = Resource.The_requested_resource_was_not_found;
                apiErrorResponse.Status = HttpStatusCode.NotFound;
                apiErrorResponse.ErrorMessage = exception.Message;

                // Include inner exception and stack trace if detailed exceptions are enabled
                apiErrorResponse.InnerException = exceptionOptions.DetailedExceptionEnabled ? exception.InnerException?.ToString() : string.Empty;
                apiErrorResponse.StackTrace = exceptionOptions.DetailedExceptionEnabled ? exception.StackTrace : string.Empty;
                break;

            // Handle InternalServerException
            case InternalServerException:
                apiErrorResponse.Type = "https://httpstatuses.com/500";
                apiErrorResponse.Title = Resource.An_internal_server_error_occurred_while_processing_your_request;
                apiErrorResponse.Status = HttpStatusCode.InternalServerError;
                apiErrorResponse.ErrorMessage = exception.Message;

                // Include inner exception and stack trace if detailed exceptions are enabled
                apiErrorResponse.InnerException = exceptionOptions.DetailedExceptionEnabled ? exception.InnerException?.ToString() : string.Empty;
                apiErrorResponse.StackTrace = exceptionOptions.DetailedExceptionEnabled ? exception.StackTrace : string.Empty;
                break;

            // Handle DbUpdateConcurrencyException
            case DbUpdateConcurrencyException:
                apiErrorResponse.Type = "https://httpstatuses.com/409";
                apiErrorResponse.Title = Resource.An_internal_server_error_occurred_while_processing_your_request;
                apiErrorResponse.Status = HttpStatusCode.Conflict;
                apiErrorResponse.ErrorMessage = Resource.It_appears_someone_else_has_made_changes_or_deleted_the_data_you_are_updating;

                // Include inner exception and stack trace if detailed exceptions are enabled
                apiErrorResponse.InnerException = exceptionOptions.DetailedExceptionEnabled ? exception.InnerException?.ToString() : string.Empty;
                apiErrorResponse.StackTrace = exceptionOptions.DetailedExceptionEnabled ? exception.StackTrace : string.Empty;
                break;

            // Handle default case
            default:
                apiErrorResponse.Type = "https://httpstatuses.com/500";
                apiErrorResponse.Title = Resource.An_error_occurred_while_processing_your_request;
                apiErrorResponse.Status = HttpStatusCode.InternalServerError;
                apiErrorResponse.ErrorMessage = exception.Message;

                // Include inner exception and stack trace if detailed exceptions are enabled
                apiErrorResponse.InnerException = exceptionOptions.DetailedExceptionEnabled ? exception.InnerException?.Message : string.Empty;
                apiErrorResponse.StackTrace = exceptionOptions.DetailedExceptionEnabled ? exception.StackTrace : string.Empty;
                break;
        }

        // Set response content type and status code
        httpContext.Response.ContentType = "application/json";
        httpContext.Response.StatusCode = (int)apiErrorResponse.Status;

        // Serialize the response to JSON
        var json = JsonSerializer.Serialize(apiErrorResponse);

        // Write the JSON response to the client
        await httpContext.Response.WriteAsync(json, cancellationToken: cancellationToken);

        // Indicate that the exception has been handled
        return true;
    }

    #endregion Public Methods
}