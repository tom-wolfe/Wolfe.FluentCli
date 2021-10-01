using FluentCli.Models;
using System;
using System.Collections.Generic;

namespace FluentCli.Core
{
    public class FluentCliCommandBuilder : IFluentCliCommandBuilder
    {
        private readonly string _name;
        private readonly Type _handlerType;
        private readonly List<FluentCliCommand> _commands = new();

        protected FluentCliCommandBuilder(string name, Type handlerType)
        {
            _name = name;
            _handlerType = handlerType;
        }

        public static IFluentCliCommandBuilder Create<THandler>(string name) where THandler : ICommandHandler => Create<THandler, object>(name);
        public static IFluentCliCommandBuilder Create<THandler, TOptions>(string name) where THandler : ICommandHandler<TOptions> => new FluentCliCommandBuilder(name, typeof(THandler));

        public FluentCliCommand Build()
        {
            return new FluentCliCommand()
            {
                Name = _name,
                Handler = _handlerType,
                SubCommands = _commands
            };
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

    public interface IFluentCliCommandBuilder
    {
        IFluentCliCommandBuilder AddCommand<THandler>(string name, Action<IFluentCliCommandBuilder> command = null) where THandler : ICommandHandler;
        FluentCliCommand Build();
    }
}
