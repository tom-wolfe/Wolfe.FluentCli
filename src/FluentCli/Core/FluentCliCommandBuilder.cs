using FluentCli.Models;
using System;
using System.Collections.Generic;

namespace FluentCli.Core
{
    public class FluentCliCommandBuilder : IFluentCliCommandBuilder, IFluentCliCommandBuilderNoOptions, IFluentCliDefaultCommandBuilder
    {
        private readonly string _name;
        private readonly Type _handlerType;
        private readonly List<FluentCliCommand> _commands = new();

        private Type _optionsType;

        protected FluentCliCommandBuilder(string name, Type handlerType)
        {
            _name = name;
            _handlerType = handlerType;
        }

        public static FluentCliCommandBuilder Create<THandler>(string name) => new (name, typeof(THandler));

        public FluentCliCommand Build() => new()
        {
            Name = _name,
            Handler = _handlerType,
            Options = _optionsType,
            SubCommands = _commands
        };

        IFluentCliDefaultCommandBuilderNoOptions IFluentCliDefaultCommandBuilder.WithOptions<TOptions>() => WithOptions<TOptions>();

        public IFluentCliCommandBuilderNoOptions WithOptions<TOptions>()
        {
            _optionsType = typeof(TOptions);
            return this;
        }

        public IFluentCliCommandBuilder AddCommand<THandler>(string name, Action<IFluentCliCommandBuilder> command = null) where THandler : ICommandHandler
        {
            var commandBuilder = Create<THandler>(name);
            command?.Invoke(commandBuilder);
            var subCommand = commandBuilder.Build();
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
        IFluentCliDefaultCommandBuilderNoOptions WithOptions<TOptions>();
    }

    public interface IFluentCliCommandBuilder : IFluentCliCommandBuilderNoOptions
    {
        IFluentCliCommandBuilderNoOptions WithOptions<TOptions>();
    }

    public interface IFluentCliCommandBuilderNoOptions : IFluentCliDefaultCommandBuilderNoOptions
    {
        IFluentCliCommandBuilder AddCommand<THandler>(string name, Action<IFluentCliCommandBuilder> command = null) where THandler : ICommandHandler;
    }
}
