using System;
using System.Collections.Generic;

namespace Wolfe.FluentCli.Models
{
    public class CliCommand
    {
        public string Name { get; init; }
        public Type Handler { get; init; }
        public CliOptions Options { get; init; }
        public List<CliCommand> SubCommands { get; init; } = new();
    }
}
