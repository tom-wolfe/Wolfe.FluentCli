using System;
using System.Threading.Tasks;
using Wolfe.FluentCli.Core;

namespace Wolfe.FluentCli.Tests.Mocks
{
    class MockCommandHandler : ICommandHandler
    {
        private readonly Action<CliContext> _handler;
        public MockCommandHandler(Action<CliContext> handler)
        {
            _handler = handler;
        }

        public Task Execute(CliContext context)
        {
            _handler(context);
            return Task.CompletedTask;
        }
    }

    class MockCommandHandler<T> : ICommandHandler<T>
    {
        private readonly Action<CliContext, T> _handler;
        public MockCommandHandler(Action<CliContext, T> handler)
        {
            _handler = handler;
        }

        public Task Execute(CliContext context, T args)
        {
            _handler(context, args);
            return Task.CompletedTask;
        }
    }
}
