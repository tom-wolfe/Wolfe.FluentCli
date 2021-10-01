using FluentCli.Exceptions;
using FluentCli.Models;
using FluentCli.Parser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace FluentCli.Core
{
    internal class FluentCli : IFluentCli
    {
        private readonly IFluentCliParser _parser;
        private readonly IServiceProvider _serviceProvider;
        private readonly FluentCliCommand _rootCommand;

        public FluentCli(IServiceProvider serviceProvider, IFluentCliParser parser, FluentCliCommand rootCommand)
        {
            _parser = parser;
            _serviceProvider = serviceProvider;
            _rootCommand = rootCommand;
        }

        public Task Execute(string args)
        {
            var instruction = _parser.Parse(args);
            var command = ResolveCommand(instruction);
            var options = ResolveOptions(instruction, command);
            return ExecuteCore(command, options);
        }

        public Task Execute(params string[] args)
        {
            var instruction = _parser.Parse(args);
            var command = ResolveCommand(instruction);
            var options = ResolveOptions(instruction, command);
            return ExecuteCore(command, options);
        }

        protected virtual FluentCliCommand ResolveCommand(FluentCliInstruction instruction)
        {
            var currentCommand = _rootCommand;
            var currentCommandChain = new List<string>();
            foreach (var commandName in instruction.Commands)
            {
                var nextCommand = currentCommand.SubCommands
                    .Find(c => c.Name.Equals(commandName, StringComparison.OrdinalIgnoreCase));
                currentCommand = nextCommand ?? throw new MissingCommandException(currentCommandChain, commandName);
                currentCommandChain.Add(commandName);
            }

            return currentCommand;
        }

        protected virtual object ResolveOptions(FluentCliInstruction instruction, FluentCliCommand command)
        {
            if (command.Options == null) { return null; }

            // Validate all required options have been passed.
            var missingRequiredOptions = command.Options.Options
                .Where(o => o.Required && !instruction.Options.Keys.Contains(o.LongName, StringComparer.OrdinalIgnoreCase))
                .Select(o => o.LongName).ToList();
            if (missingRequiredOptions.Any())
                throw new MissingRequiredCommandOptionsException(missingRequiredOptions);

            var options = command.Options?.OptionsMap?.Invoke(instruction.Options);

            return options;
        }

        protected virtual Task ExecuteCore(FluentCliCommand command, object options)
        {
            var handler = _serviceProvider.GetService(command.Handler) ??
                          throw new InvalidCommandHandlerException(command.Name);

            var handlerType = handler.GetType();

            var executeMethod = handlerType.GetMethod(nameof(ICommandHandler.Execute),
                BindingFlags.Public | BindingFlags.Instance);
            if (executeMethod == null)
                throw new InvalidCommandHandlerException(command.Name);

            var parameters = executeMethod.GetParameters();

            var invokeTask = executeMethod.Invoke(handler, parameters.Any() ? new[] { options } : null) as Task;
            return invokeTask;
        }
    }
}
