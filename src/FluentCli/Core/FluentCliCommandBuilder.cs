using FluentCli.Models;
using System;
using System.Collections.Generic;

namespace FluentCli.Core
{
    internal class FluentCliCommandBuilder : IFluentCliCommandBuilder, IFluentCliDefaultCommandBuilder
    {
        private readonly string _name;
        private readonly Type _handlerType;
        private readonly List<FluentCliCommand> _commands = new();
        private FluentCliOptions _options;

        public FluentCliCommandBuilder() : this("", null) { }

        public FluentCliCommandBuilder(string name, Type handlerType)
        {
            _name = name;
            _handlerType = handlerType;
        }

        public FluentCliCommand Build() => new()
        {
            Name = _name,
            Handler = _handlerType,
            Options = _options,
            SubCommands = _commands
        };

        IFluentCliDefaultCommandBuilderNoOptions IFluentCliDefaultCommandBuilder.WithOptions<TOptions>(Action<IFluentCliOptionsBuilder<TOptions>> options) => WithOptions(options);

        public IFluentCliCommandBuilderNoOptions WithOptions<TOptions>(Action<IFluentCliOptionsBuilder<TOptions>> options = null)
        {
            var builder = new FluentCliOptionsBuilder<TOptions>();
            options?.Invoke(builder);
            _options = builder.Build();
            return this;
        }

        public IFluentCliCommandBuilder AddCommand<THandler>(string name, Action<IFluentCliCommandBuilder> command = null) where THandler : ICommandHandler
        {
            var builder = new FluentCliCommandBuilder(name, typeof(THandler));
            command?.Invoke(builder);
            var subCommand = builder.Build();
            _commands.Add(subCommand);
            return this;
        }
    }

    public interface IFluentCliDefaultCommandBuilderNoOptions
    {
        FluentCliCommand Build();
    }

    public interface IFluentCliDefaultCommandBuilder : IFluentCliDefaultCommandBuilderNoOptions
    {
        IFluentCliDefaultCommandBuilderNoOptions WithOptions<TOptions>(Action<IFluentCliOptionsBuilder<TOptions>> options = null);
    }

    public interface IFluentCliCommandBuilder : IFluentCliCommandBuilderNoOptions
    {
        IFluentCliCommandBuilderNoOptions WithOptions<TOptions>(Action<IFluentCliOptionsBuilder<TOptions>> options = null);
    }

    public interface IFluentCliCommandBuilderNoOptions : IFluentCliDefaultCommandBuilderNoOptions
    {
        IFluentCliCommandBuilder AddCommand<THandler>(string name, Action<IFluentCliCommandBuilder> command = null) where THandler : ICommandHandler;
    }
}
