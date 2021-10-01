using System.Threading.Tasks;

namespace FluentCli
{
    public interface ICommandHandler
    {
        Task Execute();
    }
}
