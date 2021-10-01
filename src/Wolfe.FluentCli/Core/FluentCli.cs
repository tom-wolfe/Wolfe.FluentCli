using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Wolfe.FluentCli.Exceptions;
using Wolfe.FluentCli.Models;
using Wolfe.FluentCli.Parser;

namespace Wolfe.FluentCli.Core
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

            var options = cliCommand.Options.Options;
            var normalizedOptions = new Dictionary<string, string>();

            // Normalize options by long name in the original casing.
            foreach (var (key, value) in instruction.Options)
            {
                var opt = options.Find(o => o.LongName.Equals(key, StringComparison.OrdinalIgnoreCase)
                                            || o.ShortName.Equals(key, StringComparison.OrdinalIgnoreCase));
                if (opt == null) throw new InvalidCommandOptionException(cliCommand.Name, key);
                normalizedOptions.Add(opt.LongName, value);
            }

            // Validate all required options have been passed.
            var missingRequiredOptions = options
                .Where(o => o.Required && !normalizedOptions.Keys.Contains(o.LongName))
                .Select(o => o.LongName).ToList();
            if (missingRequiredOptions.Any())
                throw new MissingRequiredCommandOptionsException(missingRequiredOptions);

            return cliCommand.Options.OptionMap.CreateFrom(normalizedOptions);
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
