using System;
using System.Collections.Generic;
using Wolfe.FluentCli.Commands;
using Wolfe.FluentCli.Core.Builders;
using Wolfe.FluentCli.Core.Internal;
using Wolfe.FluentCli.Options;

namespace Wolfe.FluentCli.Internal
{
    internal class CommandBuilder : INamedCommandBuilder, ICommandBuilder
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

        public CliNamedCommand BuildNamedCommand() => new(_name, _handlerType, _options, _commands);
        public CliCommand BuildCommand() => new(_handlerType, _options, _commands);

        public INamedCommandBuilder AddCommand(string name, Action<INamedCommandBuilder> command) =>
            AddCommand<NullCommand>(name, command);

        public INamedCommandBuilder AddCommand<THandler, TArgs>(string name) =>
            AddCommand<THandler>(name, command => command.WithOptions<TArgs>());

        public INamedCommandBuilder AddCommand<THandler>(string name, Action<INamedCommandBuilder> command = null)
        {
            var builder = new CommandBuilder(name, typeof(THandler));
            command?.Invoke(builder);
            var subCommand = builder.BuildNamedCommand();
            _commands.Add(subCommand);
            return this;
        }

        ICommandBuilder ICommandBuilder.WithOptions<TArgs>(Action<IOptionsBuilder<TArgs>> options)
        {
            WithOptionsCore(options);
            return this;
        }

        INamedCommandBuilder INamedCommandBuilder.WithOptions<TArgs>(Action<IOptionsBuilder<TArgs>> options)
        {
            WithOptionsCore(options);
            return this;
        }

        private void WithOptionsCore<TArgs>(Action<IOptionsBuilder<TArgs>> options = null)
        {
            var builder = new OptionsBuilder<TArgs>();

            if (options == null)
            {
                var optionMap = new DynamicArgumentFactory<TArgs>();
                optionMap.ConfigureBuilder(builder);
                builder.UseFactory(optionMap.Create);
            }
            else
                options.Invoke(builder);
            _options = builder.Build();
        }
    }
}
