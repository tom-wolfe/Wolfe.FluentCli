using System.Collections.Generic;

namespace Wolfe.FluentCli.Parser.Models
{
    public class CliCommandDefinition
    {
        public CliArgumentDefinition Unnamed { get; init; }
        public List<CliNamedArgumentDefinition> NamedArguments { get; init; } = new();
        public List<CliNamedCommandDefinition> Commands { get; init; } = new();
    }
}
