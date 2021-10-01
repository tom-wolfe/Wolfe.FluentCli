using System;
using System.Collections.Generic;

namespace FluentCli.Models
{
    public class FluentCliOptions
    {
        public Type Model { get; init; }
        public List<FluentCliOption> Options { get; init; } = new();
    }
}
