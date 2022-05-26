using System;
using Wolfe.FluentCli.Core;
using Wolfe.FluentCli.Core.Builders;
using Wolfe.FluentCli.Exceptions;

namespace Wolfe.FluentCli
{
    public static class Cli
    {
        /// <summary>
        /// Builds a new command line interface from which commands may be executed.
        /// </summary>
        /// <param name="cli">A delegate describing how to construct the CLI.</param>
        /// <returns>The built CLI instance.</returns>
        /// <exception cref="CliBuildException">Thrown when the configuration defined by <paramref name="cli"/> would create a <see cref="IFluentCli"/> with an invalid definition. This is often due to something like duplicate command or argument names.</exception>
        public static IFluentCli Build(Action<ICliBuilder> cli)
        {
            var builder = new CliBuilder();
            cli?.Invoke(builder);
            return builder.Build();
        }
    }
}
