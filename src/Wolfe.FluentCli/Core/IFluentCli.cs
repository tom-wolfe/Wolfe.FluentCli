using System.Threading.Tasks;

namespace Wolfe.FluentCli.Core
{
    /// <summary>
    /// Represents a command-line interface that can interpret commands.
    /// </summary>
    public interface IFluentCli
    {
        /// <summary>
        /// Executes the given command based on the current CLI definition.
        /// </summary>
        /// <param name="command">A string representing the command to execute, and its associated arguments. Usually of the format: command --arg1 value1 --arg2 value2.</param>
        /// <returns>A <see cref="Task"/> object representing the execution of the command.</returns>
        /// <exception cref="CliInterpreterException">Thrown when the given <paramref name="command"/> cannot be mapped to a command and its arguments as defined within the CLI.</exception>
        /// /// <exception cref="CliExecutionException">Thrown when the command was mapped successfully, but CLI is unable to execute the command, either due to service resolution or the handler itself throwing an exception.</exception>
        Task Execute(string command);
    }
}
