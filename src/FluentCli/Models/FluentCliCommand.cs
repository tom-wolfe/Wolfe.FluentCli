using System;
using System.Collections.Generic;

namespace FluentCli.Models
{
    public class FluentCliCommand
    {
        public string Name { get; init; }
        public Type Handler { get; init; }
        public Type Options { get; init; }
        public List<FluentCliCommand> SubCommands { get; init; } = new();
    }
}
