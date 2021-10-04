using System;
using System.Collections.Generic;
using Wolfe.FluentCli.Commands;
using Wolfe.FluentCli.Models;

namespace Wolfe.FluentCli.Core
{
    internal class CommandBuilder : ICommandBuilder, IDefaultCommandBuilder
    {
        private readonly string _name;
        private readonly Type _handlerType;
        private readonly List<CliNamedCommand> _commands = new();
        private CliOptions _options;

        public CommandBuilder() : this("", null) { }

        public CommandBuilder(string name, Type handlerType)
        {
            _name = name;
            _handlerType = handlerType;
        }

        public CliNamedCommand Build() => new CliNamedCommand()
        {
            Name = _name,
            Handler = _handlerType,
            Options = _options,
            SubCommands = _commands
        };

        CliCommand IDefaultCommandBuilder.Build() => new CliCommand()
        {
            Handler = _handlerType,
            Options = _options,
            SubCommands = _commands
        };

        public ICommandBuilder AddCommand(string name, Action<ICommandBuilder> command) =>
            AddCommand<NullCommand>(name, command);

        public ICommandBuilder AddCommand<THandler, TArgs>(string name) =>
            AddCommand<THandler>(name, command => command.WithOptions<TArgs>());

        public ICommandBuilder AddCommand<THandler>(string name, Action<ICommandBuilder> command = null)
        {
            var builder = new CommandBuilder(name, typeof(THandler));
            command?.Invoke(builder);
            var subCommand = builder.Build();
            _commands.Add(subCommand);
            return this;
        }

        IDefaultCommandBuilder IDefaultCommandBuilder.WithOptions<TArgs>(Action<IOptionsBuilder<TArgs>> options)
        {
            WithOptionsCore(options);
            return this;
        }

        ICommandBuilder ICommandBuilder.WithOptions<TArgs>(Action<IOptionsBuilder<TArgs>> options)
        {
            WithOptionsCore(options);
            return this;
        }

        ICommandBuilder ICommandBuilder.WithManualOptions<TArgs>(Action<IManualOptionsBuilder<TArgs>> options)
        {
            WithManualOptionsCore(options);
            return this;
        }

        IDefaultCommandBuilder IDefaultCommandBuilder.WithManualOptions<TArgs>(Action<IManualOptionsBuilder<TArgs>> options)
        {
            WithManualOptionsCore(options);
            return this;
        }

        private void WithOptionsCore<TArgs>(Action<IOptionsBuilder<TArgs>> options = null)
        {
            var builder = new OptionsBuilder<TArgs>();
            options?.Invoke(builder);
            _options = builder.Build();
        }

        private void WithManualOptionsCore<TArgs>(Action<IManualOptionsBuilder<TArgs>> options = null)
        {
            var builder = new ManualOptionsBuilder<TArgs>();
            options?.Invoke(builder);
            _options = builder.Build();
        }
    }

    public interface ICommandBuilder
    {
        ICommandBuilder WithOptions<TArgs>(Action<IOptionsBuilder<TArgs>> options = null);
        ICommandBuilder WithManualOptions<TArgs>(Action<IManualOptionsBuilder<TArgs>> options = null);
        ICommandBuilder AddCommand(string name, Action<ICommandBuilder> command);
        ICommandBuilder AddCommand<THandler>(string name, Action<ICommandBuilder> command = null);
        ICommandBuilder AddCommand<THandler, TArgs>(string name);

        CliNamedCommand Build();
    }

    public interface IDefaultCommandBuilder
    {
        IDefaultCommandBuilder WithOptions<TArgs>(Action<IOptionsBuilder<TArgs>> options = null);
        IDefaultCommandBuilder WithManualOptions<TArgs>(Action<IManualOptionsBuilder<TArgs>> options = null);
        CliCommand Build();
    }
}
