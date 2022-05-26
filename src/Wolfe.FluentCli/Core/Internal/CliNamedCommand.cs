using System;
using System.Collections.Generic;

namespace Wolfe.FluentCli.Core.Internal
{
    internal class CliNamedCommand : CliCommand
    {
        public CliNamedCommand(string name, Type handler, CliArguments arguments, List<CliNamedCommand> commands) : base(handler, arguments, commands)
        {
            Name = name;
        }

        public string Name { get; }
    }
}
