using Wolfe.FluentCli.Core.Build.Public;

namespace Wolfe.FluentCli.Core.Build
{
    public interface IOptionsBuilder<TArgs>
    {
        IOptionsBuilder<TArgs> AddOption(string shortName, string longName, bool required);
        IOptionsBuilder<TArgs> AddOption(CliParameter parameter);
        IOptionsBuilder<TArgs> UseFactory(OptionFactory factory);
    }
}
