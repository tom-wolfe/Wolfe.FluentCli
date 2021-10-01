using System;
using System.Collections.Generic;
using Wolfe.FluentCli.Models;

namespace Wolfe.FluentCli.Core
{
    internal class CommandBuilder : ICommandBuilder, IDefaultCommandBuilder
    {
        private readonly string _name;
        private readonly Type _handlerType;
        private readonly List<CliCommand> _commands = new();
        private CliOptions _options;

        public CommandBuilder() : this("", null) { }

        public CommandBuilder(string name, Type handlerType)
        {
            _name = name;
            _handlerType = handlerType;
        }

        public CliCommand Build() => new()
        {
            Name = _name,
            Handler = _handlerType,
            Options = _options,
            SubCommands = _commands
        };

        public ICommandBuilder AddCommand<THandler>(string name, Action<ICommandBuilder> command = null)
        {
            var builder = new CommandBuilder(name, typeof(THandler));
            command?.Invoke(builder);
            var subCommand = builder.Build();
            _commands.Add(subCommand);
            return this;
        }

        IDefaultCommandBuilder IDefaultCommandBuilder.WithOptions<TOptions>(Action<IOptionsBuilder<TOptions>> options)
        {
            WithOptionsCore(options);
            return this;
        }

        ICommandBuilder ICommandBuilder.WithOptions<TOptions>(Action<IOptionsBuilder<TOptions>> options)
        {
            WithOptionsCore(options);
            return this;
        }

        ICommandBuilder ICommandBuilder.WithManualOptions<TOptions>(Action<IManualOptionsBuilder<TOptions>> options)
        {
            WithManualOptionsCore(options);
            return this;
        }

        IDefaultCommandBuilder IDefaultCommandBuilder.WithManualOptions<TOptions>(Action<IManualOptionsBuilder<TOptions>> options)
        {
            WithManualOptionsCore(options);
            return this;
        }

        private void WithOptionsCore<TOptions>(Action<IOptionsBuilder<TOptions>> options = null)
        {
            var builder = new OptionsBuilder<TOptions>();
            options?.Invoke(builder);
            _options = builder.Build();
        }

        private void WithManualOptionsCore<TOptions>(Action<IManualOptionsBuilder<TOptions>> options = null)
        {
            var builder = new ManualOptionsBuilder<TOptions>();
            options?.Invoke(builder);
            _options = builder.Build();
        }
    }

    public interface ICommandBuilder 
    {
        ICommandBuilder WithOptions<TOptions>(Action<IOptionsBuilder<TOptions>> options = null);
        ICommandBuilder WithManualOptions<TOptions>(Action<IManualOptionsBuilder<TOptions>> options = null);
        ICommandBuilder AddCommand<THandler>(string name, Action<ICommandBuilder> command = null);
        CliCommand Build();
    }

    public interface IDefaultCommandBuilder
    {
        IDefaultCommandBuilder WithOptions<TOptions>(Action<IOptionsBuilder<TOptions>> options = null);
        IDefaultCommandBuilder WithManualOptions<TOptions>(Action<IManualOptionsBuilder<TOptions>> options = null);
        CliCommand Build();
    }
}
