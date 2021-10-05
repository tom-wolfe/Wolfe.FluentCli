using Wolfe.FluentCli.Core.Models;
using Wolfe.FluentCli.Parser.Models;

namespace Wolfe.FluentCli.Parser
{
    internal interface ICliParser
    {
        CliInstruction Parse(ICliScanner scanner, CliDefinition definition);
    }
}