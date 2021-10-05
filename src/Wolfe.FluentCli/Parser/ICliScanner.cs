using Wolfe.FluentCli.Parser.Definition;

namespace Wolfe.FluentCli.Parser
{
    internal interface ICliScanner
    {
        CliToken Peek();
        CliToken Read();
    }
}
