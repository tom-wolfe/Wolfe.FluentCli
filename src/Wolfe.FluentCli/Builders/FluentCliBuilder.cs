using System;
using System.Collections.Generic;
using Wolfe.FluentCli.Commands;
using Wolfe.FluentCli.Core;
using Wolfe.FluentCli.Core.Models;
using Wolfe.FluentCli.Core.Services;
using Wolfe.FluentCli.Parser;

namespace Wolfe.FluentCli.Builders
{
    public class FluentCliBuilder : IFluentCliBuilder
    {
        private IDefaultCommandBuilder _defaultBuilder;
        private readonly List<CliNamedCommand> _subCommands = new();
        private ServiceProvider _serviceProvider = Activator.CreateInstance;

        protected FluentCliBuilder() { }

        public static IFluentCliBuilder Create() => new FluentCliBuilder();

        public IFluentCliBuilder WithServiceProvider(ServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            return this;
        }

        public IFluentCliBuilder WithDefaultCommand<THandler, TArgs>() =>
            WithDefaultCommand<THandler>(command => command.WithOptions<TArgs>());

        public IFluentCliBuilder WithDefaultCommand<THandler>(Action<IDefaultCommandBuilder> command = null)
        {
            var builder = new CommandBuilder("", typeof(THandler));
            command?.Invoke(builder);
            _defaultBuilder = builder;
            return this;
        }

        public IFluentCliBuilder AddCommand(string name, Action<ICommandBuilder> command) =>
            AddCommand<NullCommand>(name, command);

        public IFluentCliBuilder AddCommand<THandler, TArgs>(string name) =>
            AddCommand<THandler>(name, command => command.WithOptions<TArgs>());

        public IFluentCliBuilder AddCommand<THandler>(string name, Action<ICommandBuilder> command)
        {
            var cmd = BuildCommand<THandler>(name, command);
            _subCommands.Add(cmd);
            return this;
        }

        public IFluentCli Build()
        {
            var defaultBuilder = _defaultBuilder ?? new CommandBuilder();
            var defaultCommand = defaultBuilder.Build();
            defaultCommand.SubCommands.AddRange(_subCommands);

            var parser = new CliParser();
            return new Core.FluentCli(_serviceProvider, parser, defaultCommand);
        }

        private static CliNamedCommand BuildCommand<THandler>(string name, Action<ICommandBuilder> command)
        {
            var builder = new CommandBuilder(name, typeof(THandler));
            command?.Invoke(builder);
            return builder.Build();
        }
    }

    public interface IFluentCliBuilder
    {
        IFluentCliBuilder WithServiceProvider(ServiceProvider serviceProvider);
        IFluentCliBuilder WithDefaultCommand<THandler, TArgs>();
        IFluentCliBuilder WithDefaultCommand<THandler>(Action<IDefaultCommandBuilder> command = null);
        IFluentCliBuilder AddCommand(string name, Action<ICommandBuilder> command);
        IFluentCliBuilder AddCommand<THandler>(string name, Action<ICommandBuilder> command = null);
        IFluentCliBuilder AddCommand<THandler, TArgs>(string name);
        IFluentCli Build();
    }
}
