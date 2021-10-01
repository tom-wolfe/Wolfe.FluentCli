using System.Collections.Generic;
using Wolfe.FluentCli.Mapping;

namespace Wolfe.FluentCli.Models
{
    public class CliOptions
    {
        public List<CliOption> Options { get; init; } = new();
        public IOptionMap OptionMap { get; init; }
    }
}
