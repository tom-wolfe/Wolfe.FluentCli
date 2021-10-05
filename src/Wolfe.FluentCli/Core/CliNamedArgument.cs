using System.Collections.Generic;

namespace Wolfe.FluentCli.Core
{
    public class CliNamedArgument : CliArgument
    {
        public CliNamedArgument(string name, string value) : base(value) { Name = name; }
        public CliNamedArgument(string name, List<string> values) : base(values) { Name = name; }

        public string Name { get; }
    }
}
