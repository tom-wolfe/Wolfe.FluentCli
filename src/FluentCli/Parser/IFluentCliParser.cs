using FluentCli.Models;

namespace FluentCli.Parser
{
    public interface IFluentCliParser
    {
        FluentCliInstruction Parse(string args);
        FluentCliInstruction Parse(string[] args);
    }
}