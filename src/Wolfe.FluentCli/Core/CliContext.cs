using System.Collections.Generic;

namespace Wolfe.FluentCli.Core
{
    /// <summary>
    /// Represents the context within which a CLI command is executed.
    /// </summary>
    public class CliContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CliContext"/> object.
        /// </summary>
        /// <param name="cli">The CLI instance within which the command is running.</param>
        /// <param name="commands">The command hierarchy being executed.</param>
        /// <param name="unnamedArguments">Any unnamed/default arguments passed to the command.</param>
        /// <param name="namedArguments">Any named arguments/switches passed to the command, normalized by their long name.</param>
        public CliContext(IFluentCli cli, List<string> commands, CliArgument unnamedArguments, List<CliNamedArgument> namedArguments)
        {
            Cli = cli;
            Commands = commands;
            UnnamedArguments = unnamedArguments;
            NamedArguments = namedArguments;
        }

        /// <summary>
        /// Gets the CLI instance within which the command is being run.
        /// </summary>
        public IFluentCli Cli { get; }
        
        /// <summary>
        /// Gets the hierarchy of commands/subcommands being executed.
        /// </summary>
        public List<string> Commands { get; }

        /// <summary>
        /// Gets the named arguments passed to the command, normalized by their long name.
        /// </summary>
        public List<CliNamedArgument> NamedArguments { get; }

        /// <summary>
        /// Gets the unnamed/default arguments passed to the command.
        /// </summary>
        public CliArgument UnnamedArguments { get; }
    }
}
