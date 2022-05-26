using System;
using System.Collections.Generic;
using Wolfe.FluentCli.Commands;
using Wolfe.FluentCli.Core.Internal;
using Wolfe.FluentCli.Arguments;

namespace Wolfe.FluentCli.Core.Builders
{
    internal class CommandBuilder : INamedCommandBuilder, ICommandBuilder
    {
        private readonly string _name;
        private readonly Type _handlerType;
        private readonly List<CliNamedCommand> _commands = new();
        private CliArguments _arguments;

        public CommandBuilder() : this("", null) { }

        public CommandBuilder(string name, Type handlerType)
        {
            _name = name;
            _handlerType = handlerType;
        }

        public CliNamedCommand BuildNamedCommand() => new(_name, _handlerType, _arguments, _commands);
        public CliCommand BuildCommand() => new(_handlerType, _arguments, _commands);

        public INamedCommandBuilder AddCommand(string name, Action<INamedCommandBuilder> command) =>
            AddCommand<NullCommand>(name, command);

        public INamedCommandBuilder AddCommand<THandler, TArgs>(string name) =>
            AddCommand<THandler>(name, command => command.WithArguments<TArgs>());

        public INamedCommandBuilder AddCommand<THandler>(string name, Action<INamedCommandBuilder> command = null)
        {
            var builder = new CommandBuilder(name, typeof(THandler));
            command?.Invoke(builder);
            var subCommand = builder.BuildNamedCommand();
            _commands.Add(subCommand);
            return this;
        }

        ICommandBuilder ICommandBuilder.WithArguments<TArgs>(Action<IArgumentsBuilder<TArgs>> arguments)
        {
            WithArgumentsCore(arguments);
            return this;
        }

        INamedCommandBuilder INamedCommandBuilder.WithArguments<TArgs>(Action<IArgumentsBuilder<TArgs>> arguments)
        {
            WithArgumentsCore(arguments);
            return this;
        }

        private void WithArgumentsCore<TArgs>(Action<IArgumentsBuilder<TArgs>> arguments = null)
        {
            var builder = new ArgumentBuilder<TArgs>();

            if (arguments == null)
            {
                var argumentMap = new DynamicArgumentFactory<TArgs>();
                argumentMap.ConfigureBuilder(builder);
                builder.UseFactory(argumentMap.Create);
            }
            else
                arguments.Invoke(builder);
            _arguments = builder.Build();
        }
    }
}
