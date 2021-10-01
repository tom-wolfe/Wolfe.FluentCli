using System.Threading.Tasks;

namespace FluentCli
{
    public interface IFluentCli
    {
        Task Execute(string args);
        Task Execute(params string[] args);
    }
}
