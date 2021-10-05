using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Wolfe.FluentCli.Core;
using Wolfe.FluentCli.Options;

namespace Wolfe.FluentCli.Sandbox
{
    class Program
    {
        static async Task Main()
        {
            var cli = Cli.Build(cli => cli
                .WithDefaultCommand<DefaultCommandHandler, TestArgs>()
                .AddCommand("foo", hello => hello
                    .AddCommand<HelloCommandHandler, TestArgs>("hello")
                    .AddCommand<BarCommandHandler>("bar")
                )
            );

            await cli.Execute("--name Tom -a 31");
            await cli.Execute("foo");
            await cli.Execute("foo hello -n \"John Smith\"");
            await cli.Execute("foo hello unnamed -n \"John Smith\" --colors red green blue");
            await cli.Execute("foo bar");
            Console.ReadLine();
        }
    }

    public class TestArgs
    {
        [CliDefaultOption]
        public string Unnamed { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public List<string> Colors { get; set; } = new();
    }

    public class DefaultCommandHandler : ICommandHandler<TestArgs>
    {
        public Task Execute(CliContext context, TestArgs options)
        {
            Console.WriteLine($"Default: {options.Unnamed} - {options.Name} is {options.Age} years old");
            return Task.CompletedTask;
        }
    }

    public class HelloCommandHandler : ICommandHandler<TestArgs>
    {
        public Task Execute(CliContext context, TestArgs options)
        {
            Console.WriteLine($"Default: {options.Unnamed} - Hello {options.Name}! Your favorite colors are {string.Join(", ", options.Colors)}");
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
