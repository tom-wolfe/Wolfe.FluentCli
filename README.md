# Wolfe.FluentCli

A .NET 5 Fluent API for creating command line interfaces.

## Usage

### Building the CLI

```cs
var cli = FluentCliBuilder.Create()
    // The handler that runs when no command is specified.
    .WithDefaultCommand<DefaultCommandHandler>()
    .AddCommand<HelloCommandHandler>("hello", command => command
        // Options are bound automatically based on property names in snake case.
        .WithOptions<HelloCommandOptions>()
        // Commands can be nested infinitely
        .AddCommand<FooCommandHandler>("foo")
    )
    .Build();
```

### Defining Handlers

```cs
// Implement ICommandHandler when you don't have any options.
public class DefaultCommandHandler : ICommandHandler
{
    public Task Execute()
    {
        Console.WriteLine($"Default Handler");
        return Task.CompletedTask;
    }
}

public class HelloCommandOptions
{
    public string FirstName { get; set; }
}

// Implement ICommandHandler<TOptions> when you have configuration.
public class HelloCommandHandler : ICommandHandler<HelloCommandOptions>
{
    public Task Execute(HelloCommandOptions options)
    {
        Console.WriteLine($"Hello {options.FirstName}!");
        return Task.CompletedTask;
    }
}

public class FooCommandHandler : ICommandHandler
{
    public Task Execute()
    {
        Console.WriteLine("Foo");
        return Task.CompletedTask;
    }
}
```

### Executing Commands

```cs
// Executes the default command handler
await cli.Execute("");

// Executes the hello command, setting HelloCommandOptions.FirstName to 'Tom'
await cli.Execute("hello --first-name Tom");

// Executes the foo subcommand.
await cli.Execute("hello foo");
```
