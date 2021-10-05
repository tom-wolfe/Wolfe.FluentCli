using System.Collections.Generic;
using System.Linq;

namespace Wolfe.FluentCli.Core
{
    public class CliArgument
    {
        public CliArgument(string value) { Values = new List<string> { value }; }
        public CliArgument(List<string> values) { Values = values ?? new List<string>(); }

        public string Value => Values.FirstOrDefault();
        public List<string> Values { get; }
    }
}
