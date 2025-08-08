using System.Net;

namespace LinkNest.Api.Middlewares
{
    public class GlobalExceptionHandler
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionHandler> _logger;

        public GlobalExceptionHandler(RequestDelegate next, ILogger<GlobalExceptionHandler> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            try
            {
                await _next.Invoke(httpContext);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An exception was thrown and handled in GlobalExceptionHandler.");
                await HandleExceptionAsync(httpContext, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            context.Response.ContentType = "application/json";

            var (statusCode, message) = ex switch
            {
                ArgumentNullException => ((int)HttpStatusCode.BadRequest, "A required parameter is missing."),
                ArgumentException => ((int)HttpStatusCode.BadRequest, "Invalid argument provided."),
                KeyNotFoundException => ((int)HttpStatusCode.NotFound, "The requested resource was not found."),
                UnauthorizedAccessException => ((int)HttpStatusCode.Unauthorized, "Access is denied."),
                InvalidOperationException => ((int)HttpStatusCode.Conflict, "Invalid operation attempted."),
                TimeoutException => ((int)HttpStatusCode.RequestTimeout, "The request timed out."),
                NullReferenceException => ((int)HttpStatusCode.BadRequest, "A null reference occurred."),
                LinkNest.Application.Abstraction.Excecption.ValidationException =>
                    ((int)HttpStatusCode.BadRequest, "Validation failed"),
                _ => ((int)HttpStatusCode.InternalServerError, "An unexpected error occurred.")
            };

            _logger.LogError(ex, "Handled Exception: {Message} | Status Code: {StatusCode}",
                message, statusCode);

            context.Response.StatusCode = statusCode;

            object response;
            if (ex is LinkNest.Application.Abstraction.Excecption.ValidationException validationEx)
            {
                response = new
                {
                    StatusCode = statusCode,
                    Message = message,
                    Errors = validationEx.Errors.Select(e => new
                    {
                        Field = e.propertyName,
                        Error = e.errorMessage
                    })
                };
            }
            else
            {
                response = new
                {
                    StatusCode = statusCode,
                    Message = message,
                    Detail = ex.Message // ❗️احذر تظهر ده في production
                };
            }

            await context.Response.WriteAsJsonAsync(response);
        }
    }
}
