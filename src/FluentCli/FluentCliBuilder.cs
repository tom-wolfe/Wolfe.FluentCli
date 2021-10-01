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
        private Type _handler;
        private readonly List<FluentCliCommand> _subCommands = new();
        private IFluentCliParser _parser = new FluentCliParser();
        private IServiceProvider _serviceProvider = new DefaultServiceProvider();

        protected FluentCliBuilder() { }

        public static IFluentCliBuilder Create() => new FluentCliBuilder();

        public IFluentCli Build()
        {
            var rootCommand = new FluentCliCommand()
            {
                Name = "",
                Handler = _handler,
                SubCommands = _subCommands
            };
            return new Core.FluentCli(_serviceProvider, _parser, rootCommand);
        }

        public IFluentCliBuilder AddCommand<THandler>(string name, Action<IFluentCliCommandBuilder> command = null) where THandler : ICommandHandler
        {
            var builder = FluentCliCommandBuilder.Create<THandler>(name);
            command?.Invoke(builder);
            var cmd = builder.Build();
            _subCommands.Add(cmd);
            return this;
        }

        public IFluentCliBuilder WithDefaultHandler<THandler>() where THandler : ICommandHandler
        {
            _handler = typeof(THandler);
            return this;
        }

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
    }

    public interface IFluentCliBuilder
    {
        IFluentCliBuilder AddCommand<THandler>(string name, Action<IFluentCliCommandBuilder> command = null) where THandler : ICommandHandler;
        IFluentCliBuilder WithServiceProvider(IServiceProvider serviceProvider);
        IFluentCliBuilder WithParser(IFluentCliParser parser);
        IFluentCliBuilder WithDefaultHandler<THandler>() where THandler : ICommandHandler;
        IFluentCli Build();
    }
}
