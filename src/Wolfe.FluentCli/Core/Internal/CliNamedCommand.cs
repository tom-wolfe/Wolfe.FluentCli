using System;
using System.Collections.Generic;

namespace Wolfe.FluentCli.Core.Internal
{
    internal class CliNamedCommand : CliCommand
    {
        public CliNamedCommand(string name, Type handler, CliArguments options, List<CliNamedCommand> commands) : base(handler, options, commands)
        {
            Name = name;
        }

        public string Name { get; }
    }
}
