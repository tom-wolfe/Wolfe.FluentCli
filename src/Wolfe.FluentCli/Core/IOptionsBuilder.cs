using Wolfe.FluentCli.Core.Models;

namespace Wolfe.FluentCli
{
    public interface IOptionsBuilder<TArgs>
    {
        IOptionsBuilder<TArgs> AddOption(string shortName, string longName, bool required);
        IOptionsBuilder<TArgs> AddOption(CliOption option);
        IOptionsBuilder<TArgs> UseMap(OptionFactory factory);
        CliOptions Build();
    }
}
