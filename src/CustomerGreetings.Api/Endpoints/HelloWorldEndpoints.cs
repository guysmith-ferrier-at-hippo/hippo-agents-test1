using CustomerGreetings.Api.Filters;
using CustomerGreetings.Core.Models;
using CustomerGreetings.Core.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace CustomerGreetings.Api.Endpoints;

public static class HelloWorldEndpoints
{
    public static IEndpointRouteBuilder MapHelloWorldEndpoints(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet("/api/v1/hello", ([AsParameters] HelloWorldRequest request, IHelloWorldService service) =>
        {
            var response = service.GetGreeting(request.Name);
            return TypedResults.Ok(response);
        })
        .AddEndpointFilter<ValidationFilter<HelloWorldRequest>>()
        .WithName("GetHelloWorld")
        .WithSummary("Retrieve default or personalized greeting")
        .WithDescription("Retrieves a default 'Hello, World!' greeting or a personalized greeting when a recipient name is supplied.")
        .Produces<HelloWorldResponse>(StatusCodes.Status200OK, "application/json")
        .ProducesValidationProblem(StatusCodes.Status400BadRequest, "application/problem+json")
        .WithOpenApi();

        return endpoints;
    }
}
