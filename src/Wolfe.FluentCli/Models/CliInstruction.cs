using System.Collections.Generic;

namespace Wolfe.FluentCli.Models
{
    public class CliInstruction
    {
        public CliArgument Default { get; init; }
        public string[] Commands { get; init; }
        public Dictionary<string, CliArgument> Options { get; init; }
    }
}
