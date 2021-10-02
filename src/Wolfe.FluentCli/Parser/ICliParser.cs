using Wolfe.FluentCli.Models;

namespace Wolfe.FluentCli.Parser
{
    public interface ICliParser
    {
        CliInstruction Parse(string args);
    }
}