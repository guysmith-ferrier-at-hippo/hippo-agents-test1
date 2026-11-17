using CustomerGreetings.Api.Filters;
using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace CustomerGreetings.Api.IntegrationTests.Filters;

public class ValidationFilterTests
{
    private record TestModel(string? Value);

    private class TestModelValidator : AbstractValidator<TestModel>
    {
        public TestModelValidator(ValidationFailure failure)
        {
            RuleFor(x => x.Value).Custom((val, ctx) =>
            {
                ctx.AddFailure(failure);
            });
        }
    }

    [Fact]
    public async Task InvokeAsync_WhenValidatorNotRegistered_ThrowsInvalidOperationException()
    {
        // Arrange
        var services = new ServiceCollection().BuildServiceProvider();
        var httpContext = new DefaultHttpContext { RequestServices = services };
        var invocationContext = EndpointFilterInvocationContext.Create(httpContext, new TestModel("test"));

        var filter = new ValidationFilter<TestModel>();

        // Act
        var act = async () => await filter.InvokeAsync(invocationContext, _ => ValueTask.FromResult<object?>("success"));

        // Assert
        await act.Should().ThrowAsync<InvalidOperationException>();
    }

    [Fact]
    public async Task InvokeAsync_WhenPropertyNameIsEmpty_ReturnsValidationProblemWithoutError()
    {
        // Arrange
        var failure = new ValidationFailure(string.Empty, "Model level failure");
        var validator = new TestModelValidator(failure);

        var services = new ServiceCollection();
        services.AddSingleton<IValidator<TestModel>>(validator);
        var provider = services.BuildServiceProvider();

        var httpContext = new DefaultHttpContext { RequestServices = provider };
        var invocationContext = EndpointFilterInvocationContext.Create(httpContext, new TestModel("test"));

        var filter = new ValidationFilter<TestModel>();

        // Act
        var result = await filter.InvokeAsync(invocationContext, _ => ValueTask.FromResult<object?>("success"));

        // Assert
        result.Should().BeOfType<ProblemHttpResult>();
        var problem = (ProblemHttpResult)result!;
        problem.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
        problem.ProblemDetails.Detail.Should().Be("Model level failure");
    }

    [Fact]
    public async Task InvokeAsync_WhenValidationSucceeds_CallsNextDelegate()
    {
        // Arrange
        var validator = new InlineValidator<TestModel>();

        var services = new ServiceCollection();
        services.AddSingleton<IValidator<TestModel>>(validator);
        var provider = services.BuildServiceProvider();

        var httpContext = new DefaultHttpContext { RequestServices = provider };
        var invocationContext = EndpointFilterInvocationContext.Create(httpContext, new TestModel("valid"));

        var filter = new ValidationFilter<TestModel>();
        var nextCalled = false;

        // Act
        var result = await filter.InvokeAsync(invocationContext, ctx =>
        {
            nextCalled = true;
            return ValueTask.FromResult<object?>("ok");
        });

        // Assert
        nextCalled.Should().BeTrue();
        result.Should().Be("ok");
    }

    [Fact]
    public async Task InvokeAsync_WhenArgumentIsNull_CallsNextDelegateWithoutValidation()
    {
        // Arrange
        var services = new ServiceCollection().BuildServiceProvider();
        var httpContext = new DefaultHttpContext { RequestServices = services };
        var invocationContext = EndpointFilterInvocationContext.Create(httpContext, "other-type-arg");

        var filter = new ValidationFilter<TestModel>();
        var nextCalled = false;

        // Act
        var result = await filter.InvokeAsync(invocationContext, ctx =>
        {
            nextCalled = true;
            return ValueTask.FromResult<object?>("ok");
        });

        // Assert
        nextCalled.Should().BeTrue();
        result.Should().Be("ok");
    }
}
