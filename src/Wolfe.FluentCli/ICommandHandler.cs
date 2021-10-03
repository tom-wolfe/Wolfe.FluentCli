using System.Threading.Tasks;

namespace Wolfe.FluentCli
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
