using System;

namespace Wolfe.FluentCli.Core.Builders
{
    /// <summary>
    /// Defines the contract for building a new CLI.
    /// </summary>
    public interface ICliBuilder
    {
        /// <summary>
        /// Instructs the CLI builder to use a custom IoC container.
        /// </summary>
        /// <param name="serviceProvider">A factory method that constructs or retrieves an instance of the required service.</param>
        /// <returns>The current <see cref="ICliBuilder"/> instance.</returns>
        ICliBuilder WithServiceProvider(ServiceProvider serviceProvider);

        /// <summary>
        /// Specifies the default command handler that should be invoked when no other command is supplied.
        /// </summary>
        /// <typeparam name="THandler">A type that implements <see cref="ICommandHandler{TArgs}"/> that will handle the command.</typeparam>
        /// <typeparam name="TArgs">The type of arguments that the command handler accepts.</typeparam>
        /// <returns>The current <see cref="ICliBuilder"/> instance.</returns>
        ICliBuilder WithDefault<THandler, TArgs>();

        /// <summary>
        /// Specifies the default command handler that should be invoked when no other command is supplied.
        /// </summary>
        /// <typeparam name="THandler">A type that implements <see cref="ICommandHandler"/> that will handle the command.</typeparam>
        /// <param name="command">A method that describes how the command should be configured.</param>
        /// <returns>The current <see cref="ICliBuilder"/> instance.</returns>
        ICliBuilder WithDefault<THandler>(Action<ICommandBuilder> command = null);

        /// <summary>
        /// Specifies the command handler that should be invoked when the given <paramref name="name"/> is passed.
        /// </summary>
        /// <typeparam name="THandler">A type that implements <see cref="ICommandHandler"/> that will handle the command.</typeparam>
        /// <param name="name">The name used to invoke the command.</param>
        /// <param name="command">A method that describes how the command should be configured.</param>
        /// <returns>The current <see cref="ICliBuilder"/> instance.</returns>
        ICliBuilder AddCommand(string name, Action<INamedCommandBuilder> command);

        /// <summary>
        /// Specifies the command handler that should be invoked when the given <paramref name="name"/> is passed.
        /// </summary>
        /// <typeparam name="THandler">A type that implements <see cref="ICommandHandler"/> that will handle the command.</typeparam>
        /// <param name="name">The name used to invoke the command.</param>
        /// <param name="command">A method that describes how the command should be configured.</param>
        /// <returns>The current <see cref="ICliBuilder"/> instance.</returns>
        ICliBuilder AddCommand<THandler>(string name, Action<INamedCommandBuilder> command = null);

        /// <summary>
        /// Specifies the command handler that should be invoked when the given <paramref name="name"/> is passed.
        /// </summary>
        /// <typeparam name="THandler">A type that implements <see cref="ICommandHandler"/> that will handle the command.</typeparam>
        /// <typeparam name="TArgs">The type of arguments that the command handler accepts.</typeparam>
        /// <param name="name">The name used to invoke the command.</param>
        /// <param name="command">A method that describes how the command should be configured.</param>
        /// <returns>The current <see cref="ICliBuilder"/> instance.</returns>
        ICliBuilder AddCommand<THandler, TArgs>(string name);

        /// <summary>
        /// Builds the CLI and prepare it for receiving commands.
        /// </summary>
        /// <returns>The built CLI instance.</returns>
        IFluentCli Build();
    }
}
