using Wolfe.FluentCli.Parser.Definition;
using Wolfe.FluentCli.Parser.Output;

namespace Wolfe.FluentCli.Parser
{
    internal interface ICliParser
    {
        CliParseResult Parse(ICliScanner scanner, CliDefinition definition);
    }
}