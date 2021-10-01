using FluentCli.Core;
using FluentCli.Models;
using FluentCli.Parser;
using FluentCli.Service;
using System;
using System.Collections.Generic;

namespace FluentCli
{
    public class FluentCliBuilder : IFluentCliBuilder
    {
        private IFluentCliDefaultCommandBuilder _defaultBuilder ;
        private readonly List<FluentCliCommand> _subCommands = new();
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

        public IFluentCliBuilder WithDefaultCommand<THandler>(Action<IFluentCliDefaultCommandBuilder> command = null)
        {
            var builder = FluentCliCommandBuilder.Create<THandler>("");
            command?.Invoke(builder);
            _defaultBuilder = builder;
            return this;
        }

        public IFluentCliBuilder AddCommand<THandler>(string name, Action<IFluentCliCommandBuilder> command)
        {
            var cmd = BuildCommand<THandler>(name, command);
            _subCommands.Add(cmd);
            return this;
        }

        public IFluentCli Build()
        {
            var defaultBuilder = _defaultBuilder ?? FluentCliCommandBuilder.Create<object>("");
            var defaultCommand = defaultBuilder.Build();
            defaultCommand.SubCommands.AddRange(_subCommands);
            return new Core.FluentCli(_serviceProvider, _parser, defaultCommand);
        }

        private static FluentCliCommand BuildCommand<THandler>(string name, Action<IFluentCliCommandBuilder> command)
        {
            var builder = FluentCliCommandBuilder.Create<THandler>(name);
            command?.Invoke(builder);
            return builder.Build();
        }
    }

    public interface IFluentCliBuilder
    {
        IFluentCliBuilder WithServiceProvider(IServiceProvider serviceProvider);
        IFluentCliBuilder WithParser(IFluentCliParser parser);
        IFluentCliBuilder WithDefaultCommand<THandler>(Action<IFluentCliDefaultCommandBuilder> command = null);
        IFluentCliBuilder AddCommand<THandler>(string name, Action<IFluentCliCommandBuilder> command = null);
        IFluentCli Build();
    }
}
