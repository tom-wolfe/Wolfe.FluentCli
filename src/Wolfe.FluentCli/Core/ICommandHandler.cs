using System.Threading.Tasks;

namespace Wolfe.FluentCli.Core
{
    /// <summary>
    /// Represents a handler for a command, as interpreted by a <see cref="IFluentCli"/> instance.
    /// </summary>
    public interface ICommandHandler
    {
        /// <summary>
        /// Executes the command in the given context.
        /// </summary>
        /// <param name="context">The context within which the command is being executed.</param>
        /// <returns>A <see cref="Task"/> object representing the execution of the command.</returns>
        Task Execute(CliContext context);
    }

    /// <summary>
    /// Represents a handler for a command, as interpreted by a <see cref="IFluentCli"/> instance.
    /// </summary>
    /// <typeparam name="TArgs">The type of arguments passed to the command.</typeparam>
    public interface ICommandHandler<in TArgs>
    {
        /// <summary>
        /// Executes the command in the given context, with the given arguments.
        /// </summary>
        /// <param name="context">The context within which the command is being executed.</param>
        /// <param name="args">The arguments passed to the command.</param>
        /// <returns>A <see cref="Task"/> object representing the execution of the command.</returns>
        Task Execute(CliContext context, TArgs args = default);
    }
}
