namespace Wolfe.FluentCli.Core.Builders
{
    public interface IArgumentsBuilder<TArgs>
    {
        IArgumentsBuilder<TArgs> AddArgument(string shortName, string longName, bool required);
        IArgumentsBuilder<TArgs> UseFactory(ArgumentsFactory factory);
    }
}
