using System.Collections.Generic;

namespace FluentCli.Models
{
    public class CliInstruction
    {
        public string[] Commands { get; init; }
        public Dictionary<string, string> Options { get; init; }
    }
}
