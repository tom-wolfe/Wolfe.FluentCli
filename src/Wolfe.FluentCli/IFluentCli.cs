using System.Threading.Tasks;

namespace Wolfe.FluentCli
{
    public interface IFluentCli
    {
        Task Execute(string args);
        Task Execute(params string[] args);
    }
}
