using CustomerGreetings.Core.Services;
using FluentAssertions;
using Microsoft.Extensions.Time.Testing;
using Xunit;

namespace CustomerGreetings.Core.UnitTests.Services;

public class HelloWorldServiceTests
{
    private readonly FakeTimeProvider _timeProvider;
    private readonly HelloWorldService _service;
    private readonly DateTimeOffset _fixedTime;

    public HelloWorldServiceTests()
    {
        _fixedTime = DateTimeOffset.Parse("2026-03-30T12:00:00Z");
        _timeProvider = new FakeTimeProvider(_fixedTime);
        _service = new HelloWorldService(_timeProvider);
    }

    [Fact]
    public void Constructor_WhenTimeProviderIsNull_ThrowsArgumentNullException()
    {
        // Act
        var act = () => new HelloWorldService(null!);

        // Assert
        act.Should().Throw<ArgumentNullException>()
            .WithParameterName("timeProvider");
    }

    [Fact]
    public void GetGreeting_WhenNameIsNull_ReturnsDefaultGreeting()
    {
        // Act
        var response = _service.GetGreeting(null);

        // Assert
        response.Greeting.Should().Be("Hello, World!");
        response.GeneratedAt.Should().Be(_fixedTime);
    }

    [Fact]
    public void GetGreeting_WhenNoArgumentPassed_ReturnsDefaultGreeting()
    {
        // Act
        var response = _service.GetGreeting();

        // Assert
        response.Greeting.Should().Be("Hello, World!");
        response.GeneratedAt.Should().Be(_fixedTime);
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("   ")]
    public void GetGreeting_WhenNameIsEmptyOrWhitespace_ReturnsDefaultGreeting(string? name)
    {
        // Act
        var response = _service.GetGreeting(name);

        // Assert
        response.Greeting.Should().Be("Hello, World!");
        response.GeneratedAt.Should().Be(_fixedTime);
    }

    [Fact]
    public void GetGreeting_WhenNameIsValid_ReturnsPersonalizedGreeting()
    {
        // Act
        var response = _service.GetGreeting("Sarah");

        // Assert
        response.Greeting.Should().Be("Hello, Sarah!");
        response.GeneratedAt.Should().Be(_fixedTime);
    }

    [Fact]
    public void GetGreeting_WhenNameHasSurroundingWhitespace_ReturnsPersonalizedGreetingWithTrimmedName()
    {
        // Act
        var response = _service.GetGreeting("  Alex  ");

        // Assert
        response.Greeting.Should().Be("Hello, Alex!");
        response.GeneratedAt.Should().Be(_fixedTime);
    }

    [Fact]
    public void GetGreeting_WhenNameContainsUnicodeCharacters_ReturnsPersonalizedGreetingWithUnicode()
    {
        // Act
        var response = _service.GetGreeting("José");

        // Assert
        response.Greeting.Should().Be("Hello, José!");
        response.GeneratedAt.Should().Be(_fixedTime);
    }

    [Fact]
    public void GetGreeting_WhenNameIsExactly50Characters_ReturnsPersonalizedGreeting()
    {
        // Arrange
        var name = new string('a', 50);

        // Act
        var response = _service.GetGreeting(name);

        // Assert
        response.Greeting.Should().Be($"Hello, {name}!");
        response.GeneratedAt.Should().Be(_fixedTime);
    }

    [Fact]
    public void GetGreeting_AdvancesWithTimeProvider()
    {
        // Arrange
        var laterTime = _fixedTime.AddHours(2);
        _timeProvider.SetUtcNow(laterTime);

        // Act
        var response = _service.GetGreeting("World");

        // Assert
        response.GeneratedAt.Should().Be(laterTime);
    }
}
