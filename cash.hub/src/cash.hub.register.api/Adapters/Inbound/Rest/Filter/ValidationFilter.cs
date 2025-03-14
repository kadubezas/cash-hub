using System.Validation;
using cash.hub.register.api.Adapters.Inbound.Rest.Responses;

namespace cash.hub.register.api.Adapters.Inbound.Rest.Filter;

public class ValidationFilter<T>(IServiceProvider serviceProvider) : IEndpointFilter
{
    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        var validator = serviceProvider.GetRequiredService<IFlatValidator<T>>();

        if (context.Arguments.FirstOrDefault(x =>
                x?.GetType() == typeof(T)) is not T model)
        {
            return TypedResults.Problem(
                detail: "Invalid parameter.",
                statusCode: StatusCodes.Status500InternalServerError
            );
        }

        var result = await validator.ValidateAsync(model);
        if (!result)
        {
            var errorResponse = new ErrorResponse()
            {
                Code = 400,
                Message = "Requisição Inválida",
                Errors = new List<Error>()
            };

            foreach (var error in result.Errors)
            {
                errorResponse.Errors.Add(new Error(error.PropertyName, error.ErrorMessage));
            }

            return TypedResults.BadRequest(errorResponse);
        }

        return await next(context);
    }
}