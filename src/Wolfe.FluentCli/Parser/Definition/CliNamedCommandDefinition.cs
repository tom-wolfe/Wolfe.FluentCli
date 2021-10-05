using System.Collections.Generic;

namespace Wolfe.FluentCli.Parser.Definition
{
    internal class CliNamedCommandDefinition : CliCommandDefinition
    {
        public List<string> Aliases { get; } = new();
    }
}
