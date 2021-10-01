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
    public class FluentCli : IFluentCli
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
                var nextCommand =
                    currentCommand.SubCommands.Find(c =>
                        c.Name.Equals(commandName, StringComparison.OrdinalIgnoreCase));
                currentCommand = nextCommand ?? throw new MissingCommandException(currentCommandChain, commandName);
                currentCommandChain.Add(commandName);
            }

            return currentCommand;
        }

        protected virtual object ResolveOptions(FluentCliInstruction instruction, FluentCliCommand command)
        {
            var options = command.Options;
            if (options?.Model == null)
            {
                return null;
            }

            var model = _serviceProvider.GetService(options.Model) ?? Activator.CreateInstance(options.Model);

            var remainingOptions = new List<FluentCliOption>(command.Options.Options);
            var givenOptions = instruction.Options;

            foreach (var (option, value) in givenOptions)
            {
                var matchedOption = remainingOptions.Find(o =>
                    o.ShortName.Equals(option, StringComparison.OrdinalIgnoreCase) ||
                    o.LongName.Equals(option, StringComparison.OrdinalIgnoreCase));

                if (matchedOption == null)
                {
                    throw new InvalidCommandOptionException(command.Name, option);
                }

                matchedOption.Assign(model, value);
                remainingOptions.Remove(matchedOption);
            }

            var missingRequiredOptions = remainingOptions.Where(o => o.Required).Select(o => o.LongName).ToList();
            if (missingRequiredOptions.Any())
                throw new MissingRequiredCommandOptionsException(missingRequiredOptions);

            return model;
        }

        protected virtual Task ExecuteCore(FluentCliCommand command, object options)
        {
            var handler = _serviceProvider.GetService(command.Handler) ??
                          throw new InvalidCommandHandlerException(command.Name);

            var handlerType = handler.GetType();
            // TODO: Validate handler type.

            var executeMethod = handlerType.GetMethod(nameof(ICommandHandler.Execute),
                BindingFlags.Public | BindingFlags.Instance);
            if (executeMethod == null)
                throw new InvalidCommandHandlerException(command.Name);

            var parameters = executeMethod.GetParameters();

            var invokeTask = executeMethod.Invoke(handler, parameters.Any() ? new[] { options } : null) as Task;
            // TODO: Validate return task;

            return invokeTask;
        }
    }
}
