using System;
using System.Threading.Tasks;
using FluentCli.Mapping;

namespace FluentCli.Sandbox
{
    class Program
    {
        static async Task Main()
        {
            var cli = FluentCliBuilder.Create()
                .WithDefaultCommand<DefaultCommandHandler>(command => command
                    .WithOptions<DefaultCommandOptions>(options => options
                        .UseNamingStrategy(new KebabCasePropertyNamingStrategy())
                    )
                )
                .AddCommand<HelloCommandHandler>("hello", hello => hello
                    .WithManualOptions<HelloCommandOptions>(options => options
                        .AddOption("n", "name", true)
                        .UseMap(opt => new HelloCommandOptions
                        {
                            FirstName = opt["name"]
                        })
                    )
                    .AddCommand<FooCommandHandler>("foo")
                    .AddCommand<BarCommandHandler>("bar")
                )
                .Build();

            await cli.Execute("--first-name Tom");
            await cli.Execute("hello --name Tom");
            await cli.Execute("hello foo");
            await cli.Execute("hello bar");
            Console.ReadLine();
        }
    }

    public class DefaultCommandOptions
    {
        public string FirstName { get; set; }
    }

    public class DefaultCommandHandler : ICommandHandler<DefaultCommandOptions>
    {
        public Task Execute(DefaultCommandOptions options)
        {
            Console.WriteLine($"Default: {options.FirstName}");
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
