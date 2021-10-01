using FluentCli.Models;
using System;
using System.Collections.Generic;

namespace FluentCli.Core
{
    public class FluentCliCommandBuilder : IFluentCliCommandBuilder
    {
        private readonly string _name;
        private readonly Type _handlerType;
        private readonly Type _optionsType;
        private readonly List<FluentCliCommand> _commands = new();

        protected FluentCliCommandBuilder(string name, Type handlerType, Type optionsType)
        {
            _name = name;
            _handlerType = handlerType;
            _optionsType = optionsType;
        }

        public static IFluentCliCommandBuilder Create<THandler>(string name) where THandler : ICommandHandler => Create<THandler, object>(name);
        public static IFluentCliCommandBuilder Create<THandler, TOptions>(string name) where THandler : ICommandHandler<TOptions> =>
            new FluentCliCommandBuilder(name, typeof(THandler), typeof(TOptions));

        public FluentCliCommand Build() => new()
        {
            Name = _name,
            Handler = _handlerType,
            Options = _optionsType,
            SubCommands = _commands
        };

        public IFluentCliCommandBuilder AddCommand<THandler>(string name, Action<IFluentCliCommandBuilder> command = null) where THandler : ICommandHandler
        {
            var commandBuilder = Create<THandler>(name);
            command?.Invoke(commandBuilder);
            var subCommand = commandBuilder.Build();
            _commands.Add(subCommand);
            return this;
        }
    }

    public interface IFluentCliCommandBuilder
    {
        IFluentCliCommandBuilder AddCommand<THandler>(string name, Action<IFluentCliCommandBuilder> command = null) where THandler : ICommandHandler;
        FluentCliCommand Build();
    }
}
