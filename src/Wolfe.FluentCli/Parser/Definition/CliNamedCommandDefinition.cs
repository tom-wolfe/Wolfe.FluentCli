using System.Collections.Generic;

namespace Wolfe.FluentCli.Parser.Models
{
    internal class CliNamedCommandDefinition : CliCommandDefinition
    {
        public List<string> Aliases { get; } = new();
    }
}
