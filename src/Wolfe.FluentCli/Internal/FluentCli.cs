using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Wolfe.FluentCli.Core;
using Wolfe.FluentCli.Core.Build;
using Wolfe.FluentCli.Core.Build.Internal;
using Wolfe.FluentCli.Core.Run;
using Wolfe.FluentCli.Exceptions;
using Wolfe.FluentCli.Parser;
using Wolfe.FluentCli.Parser.Definition;
using Wolfe.FluentCli.Parser.Output;

namespace Wolfe.FluentCli.Internal
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
            var result = _parser.Parse(scanner, cliDefinition);
            var context = BuildContext(result);
            var command = ResolveCommand(context);
            var options = ResolveArguments(context, command);
            await ExecuteCore(context, options, command);
        }

        private async Task ExecuteCore(CliContext context, object options, CliCommand command)
        {
            var handler = _serviceProvider(command.Handler) ??
                          throw new CliExecutionException($"Unable to resolve {command.Handler.Name}from service provider.");

            var handlerType = handler.GetType();

            var executeMethod = handlerType.GetMethod(nameof(ICommandHandler.Execute),
                BindingFlags.Public | BindingFlags.Instance);
            if (executeMethod == null)
                throw new CliExecutionException($"Unable to find appropriate {command.Handler.Name}.{nameof(ICommandHandler.Execute)} method.");

            var args = new List<object> { context };

            if (executeMethod.GetParameters().Length > 1)
                args.Add(options);

            if (executeMethod.Invoke(handler, args.ToArray()) is Task invokeTask)
                await invokeTask;
        }

        private CliCommand ResolveCommand(CliContext context)
        {
            var currentCommand = _rootCliCommand;
            var currentCommandChain = new List<string>();
            foreach (var commandName in context.Commands)
            {
                var nextCommand = currentCommand.SubCommands
                    .Find(c => c.Name.Equals(commandName, StringComparison.OrdinalIgnoreCase));
                currentCommand = nextCommand ?? throw new CliInterpreterException($"Unrecognized command chain: {string.Join(' ', currentCommandChain)} {commandName}.");
                currentCommandChain.Add(commandName);
            }

            return currentCommand;
        }

        private object ResolveArguments(CliContext context, CliCommand command)
        {
            if (command.Options == null) { return null; }

            var options = command.Options.Options;

            // Validate all required options have been passed.
            var missingRequiredOptions = options
                .Where(o => o.Required && !context.NamedArguments.Any(n => n.Name == o.LongName))
                .Select(o => o.LongName).ToList();
            if (missingRequiredOptions.Any())
                throw new CliInterpreterException($"The following arguments are required: {string.Join(' ', missingRequiredOptions)}.");

            return command.Options.OptionMap(context);
        }

        private CliContext BuildContext(CliParseResult result) {
            var unnamed = new CliArgument(result.Unnamed.Values);
            var named = result.Named.Select(a => new CliNamedArgument(a.Name, a.Values)).ToList();
            return new CliContext(this, result.Commands, unnamed, named);
        }
    }
}
