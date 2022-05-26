using System.Collections.Generic;
using System.Linq;

namespace Wolfe.FluentCli.Parser.Output
{
    internal class CliParsedArgument
    {
        public CliParsedArgument(string value) { Values = new List<string> { value }; }
        public CliParsedArgument(List<string> values) { Values = values ?? new List<string>(); }

        public string Value => Values.FirstOrDefault();
        public List<string> Values { get; }
    }
}
