using System;

namespace Wolfe.FluentCli.Core.Builders
{
    public interface ICommandBuilder
    {
        ICommandBuilder WithOptions<TArgs>(Action<IOptionsBuilder<TArgs>> options = null);
    }
}
