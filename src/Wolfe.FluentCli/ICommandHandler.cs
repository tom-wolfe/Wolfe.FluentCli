using System.Threading.Tasks;

namespace Wolfe.FluentCli
{
    public interface ICommandHandler
    {
        Task Execute(CliContext context);
    }

    public interface ICommandHandler<in TOptions>
    {
        Task Execute(CliContext context, TOptions options = default);
    }
}
