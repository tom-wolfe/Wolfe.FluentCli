using System;
using System.Collections.Generic;

namespace Wolfe.FluentCli.Models
{
    public class CliCommand
    {
        public Type Handler { get; init; }
        public CliOptions Options { get; init; } = new();
        public List<CliNamedCommand> SubCommands { get; init; } = new();
    }
}
