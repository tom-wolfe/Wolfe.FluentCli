using System.Collections.Generic;
using FluentCli.Models;

namespace FluentCli.Parser
{
    public interface IFluentCliParser
    {
        CliInstruction Parse(string args);
        CliInstruction Parse(IEnumerable<string> args);
    }
}