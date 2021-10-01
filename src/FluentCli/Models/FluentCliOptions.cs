using System.Collections.Generic;

namespace FluentCli.Models
{
    public class FluentCliOptions
    {
        public List<FluentCliOption> Options { get; init; } = new();
        public OptionsMap<dynamic> OptionsMap { get; init; }
    }
}
