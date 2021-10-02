using System.Collections.Generic;
using Wolfe.FluentCli.Models;

namespace Wolfe.FluentCli.Parser
{
    public interface ICliParser
    {
        CliInstruction Parse(string args);
        CliInstruction Parse(IEnumerable<string> args);
    }
}