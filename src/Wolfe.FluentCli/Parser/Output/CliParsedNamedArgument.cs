using System.Collections.Generic;

namespace Wolfe.FluentCli.Parser.Output
{
    internal class CliParsedNamedArgument : CliParsedArgument
    {
        public CliParsedNamedArgument(string name, string value) : base(value) { Name = name; }
        public CliParsedNamedArgument(string name, List<string> values) : base(values) { Name = name; }

        public string Name { get; set; }
    }
}
