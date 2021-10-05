using System.Collections.Generic;

namespace Wolfe.FluentCli.Core.Models
{
    public class CliContext
    {
        public CliContext(IFluentCli cli, List<string> commands, CliArgument unnamedArguments, List<CliNamedArgument> namedArguments)
        {
            Cli = cli;
            Commands = commands;
            UnnamedArguments = unnamedArguments;
            NamedArguments = namedArguments;
        }
        public IFluentCli Cli { get; }
        public List<string> Commands { get; }
        public List<CliNamedArgument> NamedArguments { get; }
        public CliArgument UnnamedArguments { get; }
    }
}
