using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Wolfe.FluentCli.Core;
using Wolfe.FluentCli.Exceptions;
using Wolfe.FluentCli.Arguments;
using Xunit;

namespace Wolfe.FluentCli.Tests.E2e
{
    public class FluentCliTests
    {
        [Fact]
        public async Task Execute_DefaultCommandNoArguments()
        {
            var services = new ServiceCollection();
            var command = new Mock<ICommandHandler>();
            services.AddSingleton(command.Object);

            var provider = services.BuildServiceProvider();

            var cli = Cli.Build(cli => cli
                .WithServiceProvider(provider.GetService)
                .WithDefault<ICommandHandler>()
            );

            await cli.ExecuteAsync("");

            command.Verify(c => c.Execute(It.IsAny<CliContext>()), Times.Once);
        }

        [Fact]
        public async Task Execute_InvalidArgumentName_Throws()
        {
            var services = new ServiceCollection();
            var command = new Mock<ICommandHandler<MockArgs>>();
            services.AddSingleton(command.Object);

            var provider = services.BuildServiceProvider();

            var cli = Cli.Build(cli => cli
                .WithServiceProvider(provider.GetService)
                .WithDefault<ICommandHandler<MockArgs>, MockArgs>()
            );

            Func<Task> act = () => cli.ExecuteAsync("--bogus test1 test2");

            await act.Should().ThrowAsync<CliInterpreterException>().WithMessage("*invalid argument*");
            // command.Verify(c => c.ExecuteAsync(It.IsAny<CliContext>(), It.Is<MockArgs>(a => a.Names.Count == 2)), Times.Once);
        }

        [Fact]
        public async Task Execute_DefaultCommandDefaultArgs()
        {
            var services = new ServiceCollection();
            var command = new Mock<ICommandHandler<MockArgs>>();
            services.AddSingleton(command.Object);

            var provider = services.BuildServiceProvider();

            var cli = Cli.Build(cli => cli
                .WithServiceProvider(provider.GetService)
                .WithDefault<ICommandHandler<MockArgs>, MockArgs>()
            );

            await cli.ExecuteAsync("test1 test2");

            Expression<Func<MockArgs, bool>> args = args => args.Names[0] == "test1" && args.Names[1] == "test2";

            command.Verify(c => c.Execute(It.IsAny<CliContext>(), It.Is(args)), Times.Once);
        }
    }
    public class MockArgs
    {
        [CliDefaultArgument]
        public List<string> Names { get; set; }
    }
}
