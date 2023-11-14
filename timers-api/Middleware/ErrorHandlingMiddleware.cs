using FluentValidation;
using timers_api.domain;

namespace timers_api.Middleware
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ErrorHandlingMiddleware> _logger;
        public ErrorHandlingMiddleware(
            RequestDelegate next,
            ILogger<ErrorHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (NotFoundException ex)
            {
                context.Response.StatusCode = 404;
                context.Response.ContentType = "application/json";
                context.Response.Headers.Add("exception", "FetchDataException");
                var model = new
                {
                    ex.Message,
                    ex.Source,
                };
                await context.Response.WriteAsJsonAsync(model);
            }            
            catch (ValidationException ex)
            {
                if (context.Response.HasStarted)
                {
                    throw;
                }

                context.Response.StatusCode = 400;
                context.Response.ContentType = "application/json";
                context.Response.Headers.Add("exception", "ValidationException");

                var errors = ex.Errors.Select(x => x.ToString()).ToArray();
                var model = new
                {
                    ex.Message,
                    ex.Source,
                    Details = errors,
                };

                await context.Response.WriteAsJsonAsync(model);
            }
            catch (OperationCanceledException)
            {
                _logger.LogInformation("Request was cancelled");
                context.Response.StatusCode = 409;
            }
            catch (Exception ex)
            {
                context.Response.StatusCode = 500;
                context.Response.ContentType = "application/json";
                context.Response.Headers.Add("exception", ex.GetType().Name);
                var model = new
                {
                    ex.Message,
                    Details = ex.InnerException?.Message,
                    ex.Source,
                };
                await context.Response.WriteAsJsonAsync(model);
            }
        }
    }    
}
