﻿using System.Collections.Generic;

namespace Wolfe.FluentCli.Models
{
    public class CliInstruction
    {
        public List<string> Commands { get; init; }
        public CliArgument UnnamedArguments { get; init; }
        public List<CliNamedArgument> NamedArguments { get; init; }
    }
}
