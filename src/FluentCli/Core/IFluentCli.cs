using System.Threading.Tasks;

namespace FluentCli.Core
{
    public interface IFluentCli
    {
        Task Execute(string args);
        Task Execute(params string[] args);
    }
}
