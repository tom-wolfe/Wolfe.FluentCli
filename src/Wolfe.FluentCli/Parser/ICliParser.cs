using Wolfe.FluentCli.Core.Models;
using Wolfe.FluentCli.Parser.Models;
using Wolfe.FluentCli.Parser.Output;

namespace Wolfe.FluentCli.Parser
{
    internal interface ICliParser
    {
        CliParseResult Parse(ICliScanner scanner, CliDefinition definition);
    }
}