using System;
using System.Threading.Tasks;

namespace FluentCli.Sandbox
{
    class Program
    {
        static async Task Main()
        {
            var cli = FluentCliBuilder.Create()
                .WithDefaultCommand<DefaultCommandHandler>(def => def
                    .WithOptions<HelloCommandOptions>()
                )
                .AddCommand<HelloCommandHandler>("hello", hello => hello
                    .AddCommand<FooCommandHandler>("foo", foo => foo
                        .AddCommand<BarCommandHandler>("bar")
                    )
                    .WithOptions<HelloCommandOptions>()
                    .AddCommand<FooCommandHandler>("foo", foo => foo
                        .AddCommand<BarCommandHandler>("bar")
                    )
                    .AddCommand<BarCommandHandler>("bar", bar => bar
                        .AddCommand<FooCommandHandler>("foo")
                    )
                )
                .Build();

            await cli.Execute("");
            await cli.Execute("--name Tom");
            await cli.Execute("hello --name Tom");
            await cli.Execute("hello foo");
            await cli.Execute("hello foo bar");
            await cli.Execute("hello bar");
            await cli.Execute("hello bar foo");
            Console.ReadLine();
        }
    }

    public class DefaultCommandHandler : ICommandHandler<HelloCommandOptions>
    {
        public Task Execute(HelloCommandOptions options)
        {
            Console.WriteLine($"Default: {options.Name}");
            return Task.CompletedTask;
        }
    }

    public class HelloCommandOptions
    {
        public string Name { get; set; }
    }

    public class HelloCommandHandler : ICommandHandler<HelloCommandOptions>
    {
        public Task Execute(HelloCommandOptions options)
        {
            Console.WriteLine($"Hello {options?.Name}!");
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

    public class BarCommandHandler : ICommandHandler
    {
        public Task Execute()
        {
            Console.WriteLine("Bar");
            return Task.CompletedTask;
        }
    }
}
