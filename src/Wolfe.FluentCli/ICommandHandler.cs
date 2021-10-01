using System.Threading.Tasks;

namespace Wolfe.FluentCli
{
    public interface ICommandHandler
    {
        Task Execute();
    }

    public interface ICommandHandler<in TOptions>
    {
        Task Execute(TOptions options = default);
    }
}
