using System.Collections.Generic;

namespace Wolfe.FluentCli.Parser.Models
{
    public class CliCommandDefinition
    {
        public CliArgumentDefinition Unnamed { get; } = new();
        public List<CliNamedArgumentDefinition> NamedArguments { get; } = new();
        public List<CliNamedCommandDefinition> Commands { get; } = new();
    }
}
