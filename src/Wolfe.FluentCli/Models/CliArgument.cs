using System.Collections.Generic;
using System.Linq;

namespace Wolfe.FluentCli.Core.Models
{
    public class CliArgument
    {
        public CliArgument(string value) { Values = new List<string> { value }; }
        public CliArgument(List<string> values) { Values = values; }

        public string Value => Values.FirstOrDefault();
        public List<string> Values { get; init; }
    }
}
