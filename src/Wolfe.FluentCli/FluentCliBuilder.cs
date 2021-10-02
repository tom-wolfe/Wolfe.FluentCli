using System;
using System.Collections.Generic;
using Wolfe.FluentCli.Core;
using Wolfe.FluentCli.Models;
using Wolfe.FluentCli.Parser;

namespace Wolfe.FluentCli
{
    public class FluentCliBuilder : IFluentCliBuilder
    {
        private IDefaultCommandBuilder _defaultBuilder;
        private readonly List<CliCommand> _subCommands = new();
        private IFluentCliParser _parser = new FluentCliParser();
        private IServiceProvider _serviceProvider = new DefaultServiceProvider();

        protected FluentCliBuilder() { }

        public static IFluentCliBuilder Create() => new FluentCliBuilder();

        public IFluentCliBuilder WithParser(IFluentCliParser parser)
        {
            _parser = parser;
            return this;
        }

        public IFluentCliBuilder WithServiceProvider(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            return this;
        }

        public IFluentCliBuilder WithDefaultCommand<THandler>(Action<IDefaultCommandBuilder> command = null)
        {
            var builder = new CommandBuilder("", typeof(THandler));
            command?.Invoke(builder);
            _defaultBuilder = builder;
            return this;
        }

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
            return new Core.FluentCli(_serviceProvider, _parser, defaultCommand);
        }

        private static CliCommand BuildCommand<THandler>(string name, Action<ICommandBuilder> command)
        {
            var builder = new CommandBuilder(name, typeof(THandler));
            command?.Invoke(builder);
            return builder.Build();
        }
    }

    public interface IFluentCliBuilder
    {
        IFluentCliBuilder WithServiceProvider(IServiceProvider serviceProvider);
        IFluentCliBuilder WithParser(IFluentCliParser parser);
        IFluentCliBuilder WithDefaultCommand<THandler>(Action<IDefaultCommandBuilder> command = null);
        IFluentCliBuilder AddCommand<THandler>(string name, Action<ICommandBuilder> command = null);
        IFluentCliBuilder AddCommand<THandler, TArgs>(string name);
        IFluentCli Build();
    }
}
