using System;

namespace Wolfe.FluentCli
{
    public interface INamedCommandBuilder
    {
        INamedCommandBuilder WithOptions<TArgs>(Action<IOptionsBuilder<TArgs>> options = null);
        INamedCommandBuilder AddCommand(string name, Action<INamedCommandBuilder> command);
        INamedCommandBuilder AddCommand<THandler>(string name, Action<INamedCommandBuilder> command = null);
        INamedCommandBuilder AddCommand<THandler, TArgs>(string name);
    }
}
