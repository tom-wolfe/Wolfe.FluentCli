using System.Collections.Generic;

namespace Wolfe.FluentCli.Parser.Definition
{
    internal class CliCommandDefinition
    {
        public CliArgumentDefinition Unnamed { get; } = new();
        public List<CliNamedArgumentDefinition> NamedArguments { get; } = new();
        public List<CliNamedCommandDefinition> Commands { get; } = new();
    }
}
