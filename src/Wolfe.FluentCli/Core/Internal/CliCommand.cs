using System;
using System.Collections.Generic;

namespace Wolfe.FluentCli.Core.Internal
{
    internal class CliCommand
    {
        public CliCommand(Type handler, CliOptions options, List<CliNamedCommand> commands)
        {
            Handler = handler;
            Options = options ?? new CliOptions();
            Commands = commands ?? new List<CliNamedCommand>();
        }
        public Type Handler { get; set; }
        public CliOptions Options { get; set; } = new();
        public List<CliNamedCommand> Commands { get; set; } = new();
    }
}
