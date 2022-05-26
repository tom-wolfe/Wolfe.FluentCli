using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Threading.Tasks;
using Wolfe.FluentCli.Core;
using Wolfe.FluentCli.Arguments;

namespace Wolfe.FluentCli.Sandbox
{
    class Program
    {
        static async Task Main()
        {
            var cli = Cli.Build(cli => cli
                .WithDefault<DefaultCommandHandler, TestArgs>()
                .AddCommand("foo", hello => hello
                    .AddCommand<HelloCommandHandler>("hello"))
                    .AddCommand<BarCommandHandler>("bar")
                );

            await cli.ExecuteAsync("--name Tom -a 31");
            await cli.ExecuteAsync("foo");
            await cli.ExecuteAsync("foo hello -n \"John Smith\"");
            await cli.ExecuteAsync("foo hello unnamed -n \"John Smith\" --colors red green blue");
            await cli.ExecuteAsync("foo bar");
            Console.ReadLine();
        }
    }

    public class TestArgs
    {
        [CliDefaultArgument]
        public string Unnamed { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public List<string> Colors { get; set; } = new();
    }

    public class DefaultCommandHandler : ICommandHandler<TestArgs>
    {
        public Task Execute(CliContext context, TestArgs args)
        {
            Console.WriteLine($"Default: {args.Unnamed} - {args.Name} is {args.Age} years old");
            return Task.CompletedTask;
        }
    }

    public class HelloCommandHandler : ICommandHandler<TestArgs>
    {
        public Task Execute(CliContext context, TestArgs args)
        {
            Console.WriteLine($"Default: {args.Unnamed} - Hello {args.Name}! Your favorite colors are {string.Join(", ", args.Colors)}");
            return Task.CompletedTask;
        }
    }

    public class BarCommandHandler : ICommandHandler
    {
        public Task Execute(CliContext context)
        {
            Console.WriteLine("Bar");
            return Task.CompletedTask;
        }
    }
}
