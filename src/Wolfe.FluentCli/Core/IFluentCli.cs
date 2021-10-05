using System.Threading.Tasks;

namespace Wolfe.FluentCli.Core
{
    public interface IFluentCli
    {
        Task Execute(string args);
    }
}
