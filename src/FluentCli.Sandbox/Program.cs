using System;
using System.Threading.Tasks;

namespace FluentCli.Sandbox
{
    class Program
    {
        static async Task Main()
        {
            var cli = FluentCliBuilder.Create()
                .WithDefaultCommand<DefaultCommandHandler>(command => command
                    .WithOptions<DefaultCommandOptions>()
                )
                .AddCommand<HelloCommandHandler>("hello", hello => hello
                    .WithOptions<HelloCommandOptions>()
                    .AddCommand<FooCommandHandler>("foo")
                    .AddCommand<BarCommandHandler>("bar")
                )
                .Build();

            await cli.Execute("--first-name Tom -a 31");
            await cli.Execute("hello -n \"Joe Bloggs\"");
            await cli.Execute("hello -n \"Joe Bloggs");
            await cli.Execute("hello foo");
            await cli.Execute("hello bar");
            Console.ReadLine();
        }
    }

    public class DefaultCommandOptions
    {
        public string FirstName { get; set; }
        public int Age { get; set; }
    }

    public class DefaultCommandHandler : ICommandHandler<DefaultCommandOptions>
    {
        public Task Execute(DefaultCommandOptions options)
        {
            Console.WriteLine($"Default: {options.FirstName} is {options.Age} old");
            return Task.CompletedTask;
        }
    }

    public class HelloCommandOptions
    {
        [FluentCliOption("n", "name")]
        public string FirstName { get; set; }
    }

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

    public class BarCommandHandler : ICommandHandler
    {
        public Task Execute()
        {
            Console.WriteLine("Bar");
            return Task.CompletedTask;
        }
    }
}
