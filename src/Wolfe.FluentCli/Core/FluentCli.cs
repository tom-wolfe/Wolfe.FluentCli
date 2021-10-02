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

        public Task Execute(string args) => ExecuteInstruction(_parser.Parse(args));
        public Task Execute(params string[] args) => ExecuteInstruction(_parser.Parse(args));

        protected virtual async Task ExecuteInstruction(CliInstruction instruction)
        {
            var command = ResolveCommand(instruction);
            var context = BuildContext(instruction, command);
            var options = ResolveOptions(instruction, command);
            await ExecuteCore(context, options);
        }

        protected virtual async Task ExecuteCore(CliContext context, object options)
        {
            var handler = _serviceProvider.GetService(context.Command.Handler) ??
                          throw new CommandHandlerException(context.Command.Name, "Unable to resolve from service provider.");

            var handlerType = handler.GetType();

            var executeMethod = handlerType.GetMethod(nameof(ICommandHandler.Execute),
                BindingFlags.Public | BindingFlags.Instance);
            if (executeMethod == null)
                throw new CommandHandlerException(context.Command.Name, $"Unable to find appropriate {nameof(ICommandHandler.Execute)} method.");

            var args = new List<object> { context };

            if (executeMethod.GetParameters().Length > 1)
                args.Add(options);

            if (executeMethod.Invoke(handler, args.ToArray()) is Task invokeTask)
                await invokeTask;
        }

        protected virtual CliCommand ResolveCommand(CliInstruction instruction)
        {
            var currentCommand = _rootCliCommand;
            var currentCommandChain = new List<string>();
            foreach (var commandName in instruction.Commands)
            {
                var nextCommand = currentCommand.SubCommands
                    .Find(c => c.Name.Equals(commandName, StringComparison.OrdinalIgnoreCase));
                currentCommand = nextCommand ?? throw new CommandNotFoundException(currentCommandChain, commandName);
                currentCommandChain.Add(commandName);
            }

            return currentCommand;
        }

        protected virtual object ResolveOptions(CliInstruction instruction, CliCommand command)
        {
            if (command.Options == null) { return null; }

            var options = command.Options.Options;
            var normalizedOptions = new Dictionary<string, string>();

            // Normalize options by long name in the original casing.
            foreach (var (key, value) in instruction.Options)
            {
                var opt = options.Find(o => o.LongName.Equals(key, StringComparison.OrdinalIgnoreCase)
                                            || o.ShortName.Equals(key, StringComparison.OrdinalIgnoreCase));
                if (opt == null) throw new InvalidCommandOptionException(command.Name, key);
                normalizedOptions.Add(opt.LongName, value);
            }

            // Validate all required options have been passed.
            var missingRequiredOptions = options
                .Where(o => o.Required && !normalizedOptions.Keys.Contains(o.LongName))
                .Select(o => o.LongName).ToList();
            if (missingRequiredOptions.Any())
                throw new MissingArgumentsException(missingRequiredOptions);

            return command.Options.OptionMap.CreateFrom(normalizedOptions);
        }

        protected virtual CliContext BuildContext(CliInstruction instruction, CliCommand command)
        {
            return new CliContext
            {
                Cli = this,
                Command = command,
                Instruction = instruction
            };
        }
    }
}
