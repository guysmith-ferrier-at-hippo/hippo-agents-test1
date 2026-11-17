using CustomerGreetings.Core.Models;

namespace CustomerGreetings.Core.Services;

public interface IHelloWorldService
{
    HelloWorldResponse GetGreeting(string? name = null);
}
