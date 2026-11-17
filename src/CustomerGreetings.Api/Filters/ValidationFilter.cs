using System.Text.Json;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace CustomerGreetings.Api.Filters;

public class ValidationFilter<T> : IEndpointFilter where T : class
{
    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        var argument = context.Arguments.OfType<T>().FirstOrDefault();
        if (argument is not null)
        {
            var validator = context.HttpContext.RequestServices.GetService<IValidator<T>>();
            if (validator is not null)
            {
                var validationResult = await validator.ValidateAsync(argument, context.HttpContext.RequestAborted);
                if (!validationResult.IsValid)
                {
                    var errors = validationResult.Errors
                        .GroupBy(e => JsonNamingPolicy.CamelCase.ConvertName(e.PropertyName))
                        .ToDictionary(
                            g => g.Key,
                            g => g.Select(e => e.ErrorMessage).ToArray()
                        );

                    var firstError = validationResult.Errors.FirstOrDefault()?.ErrorMessage;

                    return Results.ValidationProblem(
                        errors: errors,
                        detail: firstError,
                        instance: context.HttpContext.Request.Path,
                        statusCode: StatusCodes.Status400BadRequest,
                        title: "One or more validation errors occurred.",
                        type: "https://tools.ietf.org/html/rfc7807"
                    );
                }
            }
        }

        return await next(context);
    }
}
