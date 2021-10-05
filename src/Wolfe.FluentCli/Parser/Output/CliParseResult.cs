using System.Collections.Generic;

namespace Wolfe.FluentCli.Parser.Output
{
    internal class CliParseResult
    {
        public CliParseResult(List<string> commands, CliParsedArgument unnamed, List<CliParsedNamedArgument> named)
        {
            Commands = commands ?? new List<string>();
            Unnamed = unnamed ?? new CliParsedArgument((string)null);
            Named = named ?? new List<CliParsedNamedArgument>();
        }

        public List<string> Commands { get; }
        public CliParsedArgument Unnamed { get; }
        public List<CliParsedNamedArgument> Named { get; }
    }
}
