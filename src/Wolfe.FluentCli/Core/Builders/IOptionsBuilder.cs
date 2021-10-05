namespace Wolfe.FluentCli.Core.Builders
{
    public interface IOptionsBuilder<TArgs>
    {
        IOptionsBuilder<TArgs> AddOption(string shortName, string longName, bool required);
        IOptionsBuilder<TArgs> UseFactory(OptionFactory factory);
    }
}
