using System.Threading.Tasks;

namespace Wolfe.FluentCli.Commands
{
    internal class NullCommand : ICommandHandler
    {
        public Task Execute(CliContext context) => Task.CompletedTask;
    }
}
