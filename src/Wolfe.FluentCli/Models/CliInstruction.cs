using System.Collections.Generic;

namespace Wolfe.FluentCli.Models
{
    public class CliInstruction
    {
        public string[] Commands { get; init; }
        public Dictionary<string, string> Options { get; init; }
    }
}
