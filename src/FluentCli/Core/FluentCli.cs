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
        private readonly CliCommand _rootCliCommand;

        public FluentCli(IServiceProvider serviceProvider, IFluentCliParser parser, CliCommand rootCliCommand)
        {
            _parser = parser;
            _serviceProvider = serviceProvider;
            _rootCliCommand = rootCliCommand;
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

        protected virtual CliCommand ResolveCommand(CliInstruction instruction)
        {
            var currentCommand = _rootCliCommand;
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

        protected virtual object ResolveOptions(CliInstruction instruction, CliCommand cliCommand)
        {
            if (cliCommand.Options == null) { return null; }

            // Validate all required options have been passed.
            var missingRequiredOptions = cliCommand.Options.Options
                .Where(o => o.Required && !instruction.Options.Keys.Contains(o.LongName, StringComparer.OrdinalIgnoreCase))
                .Select(o => o.LongName).ToList();
            if (missingRequiredOptions.Any())
                throw new MissingRequiredCommandOptionsException(missingRequiredOptions);

            var options = cliCommand.Options?.OptionMap?.CreateFrom(instruction.Options);

            return options;
        }

        protected virtual Task ExecuteCore(CliCommand cliCommand, object options)
        {
            var handler = _serviceProvider.GetService(cliCommand.Handler) ??
                          throw new InvalidCommandHandlerException(cliCommand.Name);

            var handlerType = handler.GetType();

            var executeMethod = handlerType.GetMethod(nameof(ICommandHandler.Execute),
                BindingFlags.Public | BindingFlags.Instance);
            if (executeMethod == null)
                throw new InvalidCommandHandlerException(cliCommand.Name);

            var parameters = executeMethod.GetParameters();

            var invokeTask = executeMethod.Invoke(handler, parameters.Any() ? new[] { options } : null) as Task;
            return invokeTask;
        }
    }
}
