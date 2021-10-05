using System;
using Wolfe.FluentCli.Core.Models;
using Wolfe.FluentCli.Internal;

namespace Wolfe.FluentCli
{
    public interface ICommandBuilder
    {
        ICommandBuilder WithOptions<TArgs>(Action<IOptionsBuilder<TArgs>> options = null);
    }
}
