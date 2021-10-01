using FluentCli.Models;

namespace FluentCli.Parser
{
    public interface IFluentCliParser
    {
        CliInstruction Parse(string args);
        CliInstruction Parse(string[] args);
    }
}