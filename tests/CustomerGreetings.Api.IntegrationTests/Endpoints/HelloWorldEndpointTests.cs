using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Nodes;
using CustomerGreetings.Core.Models;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Time.Testing;
using Xunit;

namespace CustomerGreetings.Api.IntegrationTests.Endpoints;

public class HelloWorldEndpointTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;
    private readonly DateTimeOffset _fixedTime;
    private readonly FakeTimeProvider _fakeTimeProvider;

    public HelloWorldEndpointTests(WebApplicationFactory<Program> factory)
    {
        _fixedTime = DateTimeOffset.Parse("2026-03-30T12:00:00Z");
        _fakeTimeProvider = new FakeTimeProvider(_fixedTime);
        _factory = factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureTestServices(services =>
            {
                services.RemoveAll<TimeProvider>();
                services.AddSingleton<TimeProvider>(_fakeTimeProvider);
            });
        });
    }

    [Fact]
    public async Task GetHello_WithoutParameters_ReturnsDefaultGreeting_TC01()
    {
        // Arrange
        var client = _factory.CreateClient();

        // Act
        var response = await client.GetAsync("/api/v1/hello");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        response.Content.Headers.ContentType?.MediaType.Should().Be("application/json");

        var content = await response.Content.ReadFromJsonAsync<HelloWorldResponse>();
        content.Should().NotBeNull();
        content!.Greeting.Should().Be("Hello, World!");
        content.GeneratedAt.Should().Be(_fixedTime);
    }

    [Fact]
    public async Task GetHello_WithValidName_ReturnsPersonalizedGreeting_TC02()
    {
        // Arrange
        var client = _factory.CreateClient();

        // Act
        var response = await client.GetAsync("/api/v1/hello?name=Sarah");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var content = await response.Content.ReadFromJsonAsync<HelloWorldResponse>();
        content.Should().NotBeNull();
        content!.Greeting.Should().Be("Hello, Sarah!");
        content.GeneratedAt.Should().Be(_fixedTime);
    }

    [Fact]
    public async Task GetHello_WithUnicodeName_ReturnsSafelyEncodedGreeting_TC03()
    {
        // Arrange
        var client = _factory.CreateClient();

        // Act
        var response = await client.GetAsync("/api/v1/hello?name=Jos%C3%A9");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var content = await response.Content.ReadFromJsonAsync<HelloWorldResponse>();
        content.Should().NotBeNull();
        content!.Greeting.Should().Be("Hello, José!");
    }

    [Fact]
    public async Task GetHello_WithNameExceeding50Characters_ReturnsProblemDetails400_TC04()
    {
        // Arrange
        var client = _factory.CreateClient();
        var longName = new string('a', 51);

        // Act
        var response = await client.GetAsync($"/api/v1/hello?name={longName}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        response.Content.Headers.ContentType?.MediaType.Should().Be("application/problem+json");

        var json = await response.Content.ReadFromJsonAsync<JsonNode>();
        json.Should().NotBeNull();
        json!["status"]?.GetValue<int>().Should().Be(400);
        json["title"]?.GetValue<string>().Should().Be("One or more validation errors occurred.");
        json["detail"]?.GetValue<string>().Should().Be("The recipient name must not exceed 50 characters.");
        json["instance"]?.GetValue<string>().Should().Be("/api/v1/hello");
        json["type"]?.GetValue<string>().Should().Be("https://tools.ietf.org/html/rfc7807");
        json["errors"]?["name"]?[0]?.GetValue<string>().Should().Be("The recipient name must not exceed 50 characters.");
    }

    [Fact]
    public async Task GetHello_With500CharacterName_ReturnsProblemDetails400WithoutStackTraces_TC05()
    {
        // Arrange
        var client = _factory.CreateClient();
        var longName = new string('x', 500);

        // Act
        var response = await client.GetAsync($"/api/v1/hello?name={longName}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        var rawContent = await response.Content.ReadAsStringAsync();
        rawContent.Should().NotContain("Exception");
        rawContent.Should().NotContain("Stack");
    }

    [Fact]
    public async Task GetHello_WithEmptyName_FallsBackToDefaultGreeting_TC06()
    {
        // Arrange
        var client = _factory.CreateClient();

        // Act
        var response = await client.GetAsync("/api/v1/hello?name=");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var content = await response.Content.ReadFromJsonAsync<HelloWorldResponse>();
        content.Should().NotBeNull();
        content!.Greeting.Should().Be("Hello, World!");
    }

    [Fact]
    public async Task GetHello_WithWhitespaceOnlyName_FallsBackToDefaultGreeting_TC07()
    {
        // Arrange
        var client = _factory.CreateClient();

        // Act
        var response = await client.GetAsync("/api/v1/hello?name=%20%20%20");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var content = await response.Content.ReadFromJsonAsync<HelloWorldResponse>();
        content.Should().NotBeNull();
        content!.Greeting.Should().Be("Hello, World!");
    }

    [Fact]
    public async Task GetHello_WithExactly50Characters_ReturnsPersonalizedGreeting_TC08()
    {
        // Arrange
        var client = _factory.CreateClient();
        var name50 = new string('b', 50);

        // Act
        var response = await client.GetAsync($"/api/v1/hello?name={name50}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var content = await response.Content.ReadFromJsonAsync<HelloWorldResponse>();
        content.Should().NotBeNull();
        content!.Greeting.Should().Be($"Hello, {name50}!");
    }

    [Fact]
    public async Task GetHello_WithNameContainingSurroundingSpaces_TrimsGreeting_TC09()
    {
        // Arrange
        var client = _factory.CreateClient();

        // Act
        var response = await client.GetAsync("/api/v1/hello?name=%20Alex%20");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var content = await response.Content.ReadFromJsonAsync<HelloWorldResponse>();
        content.Should().NotBeNull();
        content!.Greeting.Should().Be("Hello, Alex!");
    }

    [Fact]
    public async Task GetHello_PayloadFollowsCamelCaseContract()
    {
        // Arrange
        var client = _factory.CreateClient();

        // Act
        var response = await client.GetAsync("/api/v1/hello");
        var rawJson = await response.Content.ReadAsStringAsync();

        // Assert
        rawJson.Should().Contain("\"greeting\":");
        rawJson.Should().Contain("\"generatedAt\":");
        rawJson.Should().NotContain("\"Greeting\":");
        rawJson.Should().NotContain("\"GeneratedAt\":");
    }

    [Fact]
    public async Task GetHello_VerifiesSub25msLatency_TC10()
    {
        // Arrange
        var client = _factory.CreateClient();
        // Warmup call
        await client.GetAsync("/api/v1/hello");

        var stopwatch = System.Diagnostics.Stopwatch.StartNew();
        for (var i = 0; i < 20; i++)
        {
            var res = await client.GetAsync("/api/v1/hello");
            res.StatusCode.Should().Be(HttpStatusCode.OK);
        }
        stopwatch.Stop();
        var averageMs = stopwatch.ElapsedMilliseconds / 20.0;

        averageMs.Should().BeLessThan(25.0);
    }

    [Fact]
    public async Task GetOpenApi_ExposesHelloWorldContract_TC11()
    {
        // Arrange
        var client = _factory.CreateClient();

        // Act
        var response = await client.GetAsync("/openapi/v1.json");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var json = await response.Content.ReadFromJsonAsync<JsonNode>();
        json.Should().NotBeNull();

        var paths = json!["paths"];
        paths.Should().NotBeNull();
        var helloPath = paths!["/api/v1/hello"];
        helloPath.Should().NotBeNull();
        var getOp = helloPath!["get"];
        getOp.Should().NotBeNull();

        var responses = getOp!["responses"];
        responses.Should().NotBeNull();
        responses!["200"].Should().NotBeNull();
        responses!["400"].Should().NotBeNull();
    }
}
