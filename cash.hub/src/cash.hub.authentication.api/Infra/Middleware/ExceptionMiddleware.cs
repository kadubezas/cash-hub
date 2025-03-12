using cash.hub.authentication.api.Adapters.Inbound.Rest.Response;

namespace cash.hub.authentication.api.infra.Middleware;

public class ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger) 
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception exception)
        {
            logger.LogError(exception, "Exception occurred: {Message}", exception.Message);

            var error = new ErrorResponse()
            {
                Code = 500,
                Message = exception.Message
            };

            context.Response.StatusCode =
                StatusCodes.Status500InternalServerError;

            await context.Response.WriteAsJsonAsync(error);
        }
    }
}