using System;
using System.Collections.Generic;

namespace Wolfe.FluentCli.Core.Internal
{
    internal class CliCommand
    {
        public CliCommand(Type handler, CliArguments options, List<CliNamedCommand> commands)
        {
            Handler = handler;
            Options = options ?? new CliArguments();
            Commands = commands ?? new List<CliNamedCommand>();
        }
        public Type Handler { get; }
        public CliArguments Options { get; }
        public List<CliNamedCommand> Commands { get; }
    }
}
