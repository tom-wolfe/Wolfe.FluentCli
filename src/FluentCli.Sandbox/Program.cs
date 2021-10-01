using System;
using System.Threading.Tasks;

namespace FluentCli.Sandbox
{
    class Program
    {
        static async Task Main()
        {
            var cli = FluentCliBuilder.Create()
                .WithHandler<DefaultCommandHandler>()
                .AddCommand<HelloCommandHandler>("hello", cmd => cmd
                    .AddCommand<FooCommandHandler>("foo")
                    .AddCommand<BarCommandHandler>("bar")
                )
                .Build();

            await cli.Execute("");
            await cli.Execute("hello");
            await cli.Execute("hello foo");
            await cli.Execute("hello bar");
            Console.ReadLine();
        }
    }

    public class DefaultCommandHandler : ICommandHandler
    {
        public Task Execute()
        {
            Console.WriteLine("Default");
            return Task.CompletedTask;
        }
    }

    public class HelloCommandHandler : ICommandHandler
    {
        public Task Execute()
        {
            Console.WriteLine("Hello!");
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
