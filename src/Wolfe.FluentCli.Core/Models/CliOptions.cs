using System.Collections.Generic;

namespace Wolfe.FluentCli.Core.Models
{
    public class CliOptions
    {
        public List<CliOption> Options { get; init; } = new();
        public OptionFactory OptionMap { get; init; }
    }

    public delegate object OptionFactory(Dictionary<string, CliArgument> values);
}
