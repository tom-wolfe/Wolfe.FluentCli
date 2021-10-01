using System;

namespace FluentCli.Models
{
    public class FluentCliOption
    {
        public string ShortName { get; init; }
        public string LongName { get; init; }
        public string Description { get; init; }
        public bool Required { get; init; }
        public Action<dynamic, string> Assign { get; init; }
    }
}
