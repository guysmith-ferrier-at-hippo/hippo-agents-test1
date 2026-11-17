using CustomerGreetings.Core.Models;
using CustomerGreetings.Core.Validators;
using FluentAssertions;
using Xunit;

namespace CustomerGreetings.Core.UnitTests.Validators;

public class HelloWorldRequestValidatorTests
{
    private readonly HelloWorldRequestValidator _validator = new();

    [Fact]
    public void Validate_WhenNameIsNull_ShouldBeValid()
    {
        // Arrange
        var request = new HelloWorldRequest(null);

        // Act
        var result = _validator.Validate(request);

        // Assert
        result.IsValid.Should().BeTrue();
        result.Errors.Should().BeEmpty();
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("   ")]
    public void Validate_WhenNameIsEmptyOrWhitespace_ShouldBeValid(string? name)
    {
        // Arrange
        var request = new HelloWorldRequest(name);

        // Act
        var result = _validator.Validate(request);

        // Assert
        result.IsValid.Should().BeTrue();
        result.Errors.Should().BeEmpty();
    }

    [Fact]
    public void Validate_WhenNameIsUnder50Characters_ShouldBeValid()
    {
        // Arrange
        var request = new HelloWorldRequest("Sarah");

        // Act
        var result = _validator.Validate(request);

        // Assert
        result.IsValid.Should().BeTrue();
        result.Errors.Should().BeEmpty();
    }

    [Fact]
    public void Validate_WhenNameIsExactly50Characters_ShouldBeValid()
    {
        // Arrange
        var name = new string('a', 50);
        var request = new HelloWorldRequest(name);

        // Act
        var result = _validator.Validate(request);

        // Assert
        result.IsValid.Should().BeTrue();
        result.Errors.Should().BeEmpty();
    }

    [Fact]
    public void Validate_WhenNameExceeds50Characters_ShouldFailWithExpectedErrorMessage()
    {
        // Arrange
        var name = new string('a', 51);
        var request = new HelloWorldRequest(name);

        // Act
        var result = _validator.Validate(request);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle()
            .Which.ErrorMessage.Should().Be("The recipient name must not exceed 50 characters.");
    }

    [Fact]
    public void Validate_WhenNameIs500Characters_ShouldFailWithExpectedErrorMessage()
    {
        // Arrange
        var name = new string('x', 500);
        var request = new HelloWorldRequest(name);

        // Act
        var result = _validator.Validate(request);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle()
            .Which.ErrorMessage.Should().Be("The recipient name must not exceed 50 characters.");
    }
}
