using System.Collections.Generic;

namespace Wolfe.FluentCli.Parser.Models
{
    public class CliNamedCommandDefinition : CliCommandDefinition
    {
        public List<string> Aliases { get; init; } = new();
    }
}
