using System;

namespace Wolfe.FluentCli.Core.Builders
{
    /// <summary>
    /// Defines the contract for building a new command that can make up part of a CLI.
    /// </summary>
    public interface ICommandBuilder
    {
        /// <summary>
        /// Specifies that the given type should be used as the arguments for the command.
        /// </summary>
        /// <typeparam name="TArgs">The type of arguments that the command handler accepts.</typeparam>
        /// <param name="args">A method that defines how the arguments should be configured.</param>
        /// <returns>The current instance of <see cref="ICommandBuilder"/>.</returns>
        ICommandBuilder WithArguments<TArgs>(Action<IArgumentsBuilder<TArgs>> args = null);
    }
}
