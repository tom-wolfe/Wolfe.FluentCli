using FluentCli.Models;
using System;
using System.Collections.Generic;

namespace FluentCli.Core
{
    public class FluentCliCommandBuilder : IFluentCliCommandBuilder, IFluentCliDefaultCommandBuilder
    {
        private readonly string _name;
        private readonly Type _handlerType;
        private readonly List<FluentCliCommand> _commands = new();
        private FluentCliOptions _options;

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
            Options = _options,
            SubCommands = _commands
        };

        IFluentCliDefaultCommandBuilderNoOptions IFluentCliDefaultCommandBuilder.WithOptions<TOptions>(Action<IFluentCliOptionsBuilder<TOptions>> options) => WithOptions(options);

        public IFluentCliCommandBuilderNoOptions WithOptions<TOptions>(Action<IFluentCliOptionsBuilder<TOptions>> options = null)
        {
            var builder = FluentCliOptionsBuilder<TOptions>.Create();
            options?.Invoke(builder);
            _options = builder.Build();
            return this;
        }

        public IFluentCliCommandBuilder AddCommand<THandler>(string name, Action<IFluentCliCommandBuilder> command = null) where THandler : ICommandHandler
        {
            var builder = Create<THandler>(name);
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
