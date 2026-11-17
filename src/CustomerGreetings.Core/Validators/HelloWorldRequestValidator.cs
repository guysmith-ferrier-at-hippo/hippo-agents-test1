using CustomerGreetings.Core.Models;
using FluentValidation;

namespace CustomerGreetings.Core.Validators;

public class HelloWorldRequestValidator : AbstractValidator<HelloWorldRequest>
{
    public HelloWorldRequestValidator()
    {
        RuleFor(x => x.Name)
            .MaximumLength(50)
            .WithMessage("The recipient name must not exceed 50 characters.");
    }
}
