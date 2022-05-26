# Wolfe.FluentCli

A .NET 5 Fluent API for creating command line interfaces.

## Basic Usage

### Building a CLI

A CLI object is a mediator that invokes a named command handler based on a string input, along with any required parameters. The first step is defining what the valid command structure is, and what arguments, if any, each command requires. The entire process starts with the static `Cli.Build` method. This method takes a lambda that allows you to construct a CLI interpreter object in any way you need.

Here is a basic example:

```cs
var cli = Cli.Build(cli => cli
    // The handler that runs when no command is specified.
    .WithDefault<DefaultCommand>()
    .AddCommand<HelloCommand>("hello", command => command
        // Options are bound automatically based on property names in snake case.
        .WithOptions<HelloCommandArguments>()
        // Commands can be nested infinitely
        .AddCommand<FooCommandHandler>("foo")
    ));
```

### Command Handlers

Command handlers implement the `ICommandHandler` interface or the `ICommandHandler<TArgs>` interface if they take any additional parameters. Here are the handlers for the above CLI definition:

```cs
// Implement ICommandHandler when you don't have any arguments.
public class DefaultCommand : ICommandHandler
{
    public Task Execute()
    {
        Console.WriteLine($"Default Handler");
        return Task.CompletedTask;
    }
}

// Implement ICommandHandler<TArgs> when you have configuration.
public class HelloCommand : ICommandHandler<HelloCommandArguments>
{
    public Task Execute(HelloCommandArguments arguments)
    {
        Console.WriteLine($"Hello {arguments.FirstName}!");
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

### Arguments

When your command handlers take additional values, you can define an arguments model. By default, properties are mapped using snake case. The property `FirstName` would be referenced using `--first-name` in the command line string.

If you want to override this behaviour, you can use the `CliArgument` attribute to override the short name, long name, and whether or not the argument is required. Where an argument is unnamed, you can use the `CliDefaultArgument` attribute.

```cs
public class HelloCommandArguments
{
    public string FirstName { get; set; }

    [CliArgument("l", "last-name", true)]
    public string LastName { get; set; }
}
```

### Advanced Usage

#### Dependency Injection

By default, command handlers will be instantiated using `Activator.CreateInstance`. To provide a custom dependency resoltion, you can use the `WithServiceProvider` method on the `ICliBuilder` object:

```cs
var cli = Cli.Build(cli => cli
    .WithDefault<DefaultCommandHandler, TestArgs>()
    .WithServiceProvider(x => myDIContainer.ResolveType(x))
    .AddCommand("foo", hello => hello
        .AddCommand<HelloCommandHandler>("hello"))
        .AddCommand<BarCommandHandler>("bar")
    );
```

### Executing Commands

```cs
// Executes the default command handler
await cli.Execute("");

// Executes the hello command, setting HelloCommandArguments.FirstName to 'Tom'
await cli.Execute("hello --first-name Tom");

// Executes the foo subcommand.
await cli.Execute("hello foo");
```
