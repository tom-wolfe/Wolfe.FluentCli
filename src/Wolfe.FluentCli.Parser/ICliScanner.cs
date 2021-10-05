using Wolfe.FluentCli.Parser.Models;

namespace Wolfe.FluentCli.Parser
{
    public interface ICliScanner
    {
        CliToken Peek();
        CliToken Read();
    }
}
