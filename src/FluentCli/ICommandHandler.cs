using System.Threading.Tasks;

namespace FluentCli
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
