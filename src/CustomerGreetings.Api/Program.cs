using CustomerGreetings.Api.Endpoints;
using CustomerGreetings.Core.Services;
using CustomerGreetings.Core.Validators;
using FluentValidation;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddProblemDetails();
builder.Services.AddSingleton(TimeProvider.System);
builder.Services.AddSingleton<IHelloWorldService, HelloWorldService>();
builder.Services.AddValidatorsFromAssemblyContaining<HelloWorldRequestValidator>();

builder.Services.AddOpenApi();

var app = builder.Build();

app.UseExceptionHandler();
app.MapOpenApi();
app.MapHelloWorldEndpoints();

app.Run();

public partial class Program { }
