using System;
using System.Threading.Tasks;
using Wolfe.FluentCli.Core;
using Wolfe.FluentCli.Tests.Mocks;
using Xunit;

namespace Wolfe.FluentCli.Tests.E2e
{
    public class FluentCliTests
    {
        [Fact]
        public async Task DefaultCommand()
        {
            var wasCalled = false;
            var provider = CreateMockProvider(context =>
            {
                wasCalled = true;
            });

            var cli = Cli.Build(cli => cli.WithServiceProvider(provider).WithDefault<MockCommandHandler>());

            await cli.Execute("");

            Assert.True(wasCalled);
        }

        private static ServiceProvider CreateMockProvider(Action<CliContext> handler) => (Type t) => new MockCommandHandler(handler);
    }
}
