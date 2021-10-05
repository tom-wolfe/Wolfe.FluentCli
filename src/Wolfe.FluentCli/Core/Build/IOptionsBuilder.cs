using Wolfe.FluentCli.Core;

namespace Wolfe.FluentCli
{
    public interface IOptionsBuilder<TArgs>
    {
        IOptionsBuilder<TArgs> AddOption(string shortName, string longName, bool required);
        IOptionsBuilder<TArgs> AddOption(CliParameter parameter);
        IOptionsBuilder<TArgs> UseFactory(OptionFactory factory);
    }
}
