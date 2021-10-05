using System;
using Wolfe.FluentCli.Core;
using Wolfe.FluentCli.Core.Services;

namespace Wolfe.FluentCli
{
    public interface ICliBuilder
    {
        ICliBuilder WithServiceProvider(ServiceProvider serviceProvider);
        ICliBuilder WithDefaultCommand<THandler, TArgs>();
        ICliBuilder WithDefaultCommand<THandler>(Action<ICommandBuilder> command = null);
        ICliBuilder AddCommand(string name, Action<INamedCommandBuilder> command);
        ICliBuilder AddCommand<THandler>(string name, Action<INamedCommandBuilder> command = null);
        ICliBuilder AddCommand<THandler, TArgs>(string name);
        IFluentCli Build();
    }
}
