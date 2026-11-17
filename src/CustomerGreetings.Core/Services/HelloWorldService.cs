using CustomerGreetings.Core.Models;

namespace CustomerGreetings.Core.Services;

public class HelloWorldService(TimeProvider timeProvider) : IHelloWorldService
{
    private readonly TimeProvider _timeProvider = timeProvider ?? throw new ArgumentNullException(nameof(timeProvider));

    public HelloWorldResponse GetGreeting(string? name = null)
    {
        var greeting = string.IsNullOrWhiteSpace(name)
            ? "Hello, World!"
            : $"Hello, {name.Trim()}!";

        return new HelloWorldResponse(greeting, _timeProvider.GetUtcNow());
    }
}
