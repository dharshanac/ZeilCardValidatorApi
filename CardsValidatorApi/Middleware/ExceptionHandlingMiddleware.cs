using ZeilCardValidatorApi.Domain.Exceptions;

namespace ZeilCardValidatorApi.CardsValidatorApi.Middleware
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;
        private readonly IHostEnvironment _env;

        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger, IHostEnvironment env)
        {
            _next = next;
            _logger = logger;
            _env = env;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (FluentValidation.ValidationException ex)
            {
                _logger.LogWarning(ex, "Validation error");
                var errors = ex.Errors
                    .GroupBy(e => e.PropertyName)
                    .ToDictionary(g => g.Key, g => g.Select(e => e.ErrorMessage).ToArray());

                context.Response.StatusCode = 400;
                await context.Response.WriteAsJsonAsync(new
                {
                    type = "https://httpstatuses.com/400",
                    title = "Validation Failed",
                    status = 400,
                    errors,
                    traceId = context.TraceIdentifier
                });
            }
            catch (DomainValidationException ex)
            {
                _logger.LogWarning(ex, "Domain validation error");
                context.Response.StatusCode = 400;
                await context.Response.WriteAsJsonAsync(new
                {
                    type = "https://httpstatuses.com/400",
                    title = ex.GetType().Name,
                    status = 400,
                    detail = ex.Message,
                    errorCode = ex.ErrorCode,
                    traceId = context.TraceIdentifier,
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error");
                context.Response.StatusCode = 500;
                await context.Response.WriteAsJsonAsync(new
                {
                    type = "https://httpstatuses.com/500",
                    title = "Internal Server Error",
                    status = 500,
                    detail = ex.Message,
                    traceId = context.TraceIdentifier,
                    stackTrace = _env.IsDevelopment() ? ex.StackTrace : null
                });
            }
        }
    }
}
