using System.Collections.Generic;
using FluentCli.Mapping;

namespace FluentCli.Models
{
    public class CliOptions
    {
        public List<CliOption> Options { get; init; } = new();
        public IOptionMap OptionMap { get; init; }
    }
}
