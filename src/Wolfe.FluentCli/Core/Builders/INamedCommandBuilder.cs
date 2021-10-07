using System;

namespace Wolfe.FluentCli.Core.Builders
{
    /// <summary>
    /// Defines the contract for building a named command.
    /// </summary>
    public interface INamedCommandBuilder
    {
        INamedCommandBuilder WithOptions<TArgs>(Action<IArgumentsBuilder<TArgs>> options = null);

        /// <summary>
        /// Adds a subcommand with no handler that acts as a named grouping of subcommands.
        /// </summary>
        /// <param name="name">The name used to invoke the command.</param>
        /// <param name="command">A method that describes how the subcommands should be configured.</param>
        /// <returns>The current <see cref="INamedCommandBuilder"/> instance.</returns>
        INamedCommandBuilder AddCommand(string name, Action<INamedCommandBuilder> command);

        /// <summary>
        /// Adds a subcommand handler that should be invoked when the given <paramref name="name"/> is passed.
        /// </summary>
        /// <typeparam name="THandler">A type that implements <see cref="ICommandHandler"/> that will handle the command.</typeparam>
        /// <param name="name">The name used to invoke the command.</param>
        /// <param name="command">A method that describes how the command should be configured.</param>
        /// <returns>The current <see cref="INamedCommandBuilder"/> instance.</returns>
        INamedCommandBuilder AddCommand<THandler>(string name, Action<INamedCommandBuilder> command = null);

        /// <summary>
        /// Adds a subcommand handler that should be invoked when the given <paramref name="name"/> is passed.
        /// </summary>
        /// <typeparam name="THandler">A type that implements <see cref="ICommandHandler"/> that will handle the command.</typeparam>
        /// <typeparam name="TArgs">The type of arguments that the command handler accepts.</typeparam>
        /// <param name="name">The name used to invoke the command.</param>
        /// <returns>The current <see cref="INamedCommandBuilder"/> instance.</returns>
        INamedCommandBuilder AddCommand<THandler, TArgs>(string name);
    }
}
