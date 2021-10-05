using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Wolfe.FluentCli.Core.Exceptions;
using Wolfe.FluentCli.Core.Models;
using Wolfe.FluentCli.Core.Services;
using Wolfe.FluentCli.Parser;
using Wolfe.FluentCli.Parser.Models;

namespace Wolfe.FluentCli.Core
{
    internal class FluentCli : IFluentCli
    {
        private readonly ICliParser _parser;
        private readonly ServiceProvider _serviceProvider;
        private readonly CliCommand _rootCliCommand;

        public FluentCli(ServiceProvider serviceProvider, ICliParser parser, CliCommand rootCliCommand)
        {
            _parser = parser;
            _serviceProvider = serviceProvider;
            _rootCliCommand = rootCliCommand;
        }

        public async Task Execute(string args)
        {
            var scanner = new CliScanner(args);
            var cliDefinition = CliDefinition.FromCommand(_rootCliCommand);
            var instruction = _parser.Parse(scanner, cliDefinition);
            var command = ResolveCommand(instruction);
            var context = BuildContext(instruction, command);
            var options = ResolveOptions(instruction, command);
            await ExecuteCore(context, options);
        }

        protected virtual async Task ExecuteCore(CliContext context, object options)
        {
            var handler = _serviceProvider(context.Command.Handler) ??
                          throw new CliExecutionException($"Unable to resolve {context.Command.Handler.Name}from service provider.");

            var handlerType = handler.GetType();

            var executeMethod = handlerType.GetMethod(nameof(ICommandHandler.Execute),
                BindingFlags.Public | BindingFlags.Instance);
            if (executeMethod == null)
                throw new CliExecutionException($"Unable to find appropriate {context.Command.Handler.Name}.{nameof(ICommandHandler.Execute)} method.");

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
                currentCommand = nextCommand ?? throw new CliInterpreterException($"Unrecognized command chain: {string.Join(' ', currentCommandChain)} {commandName}.");
                currentCommandChain.Add(commandName);
            }

            return currentCommand;
        }

        protected virtual object ResolveOptions(CliInstruction instruction, CliCommand command)
        {
            if (command.Options == null) { return null; }

            var options = command.Options.Options;
            var normalizedOptions = new Dictionary<string, CliArgument>();

            // Normalize options by long name in the original casing.
            foreach (var argument in instruction.NamedArguments)
            {
                var opt = options.Find(o => o.LongName.Equals(argument.Name, StringComparison.OrdinalIgnoreCase)
                                            || o.ShortName.Equals(argument.Name, StringComparison.OrdinalIgnoreCase));
                if (opt == null) throw new CliInterpreterException($"Unrecognized argument {argument.Name} for command {command.Handler.Name}");
                normalizedOptions.Add(opt.LongName, argument);
            }

            // Validate all required options have been passed.
            var missingRequiredOptions = options
                .Where(o => o.Required && !normalizedOptions.Keys.Contains(o.LongName))
                .Select(o => o.LongName).ToList();
            if (missingRequiredOptions.Any())
                throw new CliInterpreterException($"The following arguments are required: {string.Join(' ', missingRequiredOptions)}.");

            return command.Options.OptionMap(normalizedOptions);
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
