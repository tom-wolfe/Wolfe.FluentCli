using System;
using System.Threading.Tasks;
using Wolfe.FluentCli.Core;
using Wolfe.FluentCli.Core.Models;
using Wolfe.FluentCli.Options;

namespace Wolfe.FluentCli.Sandbox
{
    class Program
    {
        static async Task Main()
        {
            var cli = Cli.Build(cli => cli
                .WithDefaultCommand<DefaultCommandHandler, DefaultCommandOptions>()
                .AddCommand("foo", hello => hello
                    .AddCommand<HelloCommandHandler, HelloCommandOptions>("hello")
                    .AddCommand<BarCommandHandler>("bar")
                )
            );

            await cli.Execute("--first-name Tom -a 31 years old");
            await cli.Execute("foo");
            await cli.Execute("foo hello -n \"Joe Bloggs\"");
            await cli.Execute("foo hello -n \"Joe Bloggs");
            await cli.Execute("foo bar");
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
        public Task Execute(CliContext context, DefaultCommandOptions options)
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
        public Task Execute(CliContext context, HelloCommandOptions options)
        {
            Console.WriteLine($"Hello {options.FirstName}!");
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
