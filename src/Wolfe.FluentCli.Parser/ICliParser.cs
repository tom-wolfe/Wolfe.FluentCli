using Wolfe.FluentCli.Core.Models;
using Wolfe.FluentCli.Parser.Models;

namespace Wolfe.FluentCli.Parser
{
    public interface ICliParser
    {
        CliInstruction Parse(ICliScanner scanner, CliDefinition definition);
    }
}