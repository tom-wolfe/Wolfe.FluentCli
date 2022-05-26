using System;
using System.Collections.Generic;

namespace Wolfe.FluentCli.Core.Internal
{
    internal class CliCommand
    {
        public CliCommand(Type handler, CliArguments arguments, List<CliNamedCommand> commands)
        {
            Handler = handler;
            Arguments = arguments ?? new CliArguments();
            Commands = commands ?? new List<CliNamedCommand>();
        }
        public Type Handler { get; }
        public CliArguments Arguments { get; }
        public List<CliNamedCommand> Commands { get; }
    }
}
