using System.Threading.Tasks;

namespace Wolfe.FluentCli.Core
{
    public interface ICommandHandler
    {
        Task Execute(CliContext context);
    }

    public interface ICommandHandler<in TArgs>
    {
        Task Execute(CliContext context, TArgs options = default);
    }
}
