using Wolfe.FluentCli.Core.Models;

namespace Wolfe.FluentCli
{
    public class CliContext
    {
        public CliCommand Command { get; init; }
        public CliInstruction Instruction { get; init; }
        public IFluentCli Cli { get; init; }
    }
}
