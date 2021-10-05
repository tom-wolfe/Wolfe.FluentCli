namespace Wolfe.FluentCli.Core.Models
{
    public class CliContext
    {
        public CliCommand Command { get; init; }
        public CliInstruction Instruction { get; init; }
        public IFluentCli Cli { get; init; }
    }
}
