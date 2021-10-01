using System.Threading.Tasks;

namespace FluentCli
{
    public interface ICommandHandler : ICommandHandler<object>
    {

    }

    public interface ICommandHandler<in TOptions>
    {
        Task Execute(TOptions options = default);
    }
}
