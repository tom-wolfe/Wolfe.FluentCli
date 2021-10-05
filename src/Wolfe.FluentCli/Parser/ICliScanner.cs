using Wolfe.FluentCli.Parser.Models;

namespace Wolfe.FluentCli.Parser
{
    internal interface ICliScanner
    {
        CliToken Peek();
        CliToken Read();
    }
}
