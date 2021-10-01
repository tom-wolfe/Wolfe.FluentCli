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
        private Type _handlerType;
        private Type _optionsType;
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
                Handler = _handlerType,
                Options = _optionsType,
                SubCommands = _subCommands
            };
            return new Core.FluentCli(_serviceProvider, _parser, rootCommand);
        }

        public IFluentCliBuilder AddCommand<THandler>(string name, Action<IFluentCliCommandBuilder> command = null) where THandler : ICommandHandler =>
            AddCommand<THandler, object>(name, command);

        public IFluentCliBuilder AddCommand<THandler, TOptions>(string name, Action<IFluentCliCommandBuilder> command = null) where THandler : ICommandHandler<TOptions>
        {
            var builder = FluentCliCommandBuilder.Create<THandler, TOptions>(name);
            command?.Invoke(builder);
            var cmd = builder.Build();
            _subCommands.Add(cmd);
            return this;
        }

        public IFluentCliBuilder WithDefaultHandler<THandler>() where THandler : ICommandHandler =>
            WithDefaultHandler<THandler, object>();

        public IFluentCliBuilder WithDefaultHandler<THandler, TOptions>() where THandler : ICommandHandler<TOptions>
        {
            _handlerType = typeof(THandler);
            _optionsType = typeof(TOptions);
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
        IFluentCliBuilder AddCommand<THandler, TOptions>(string name, Action<IFluentCliCommandBuilder> command = null) where THandler : ICommandHandler<TOptions>;
        IFluentCliBuilder WithServiceProvider(IServiceProvider serviceProvider);
        IFluentCliBuilder WithParser(IFluentCliParser parser);
        IFluentCliBuilder WithDefaultHandler<THandler>() where THandler : ICommandHandler;
        IFluentCli Build();
    }
}
