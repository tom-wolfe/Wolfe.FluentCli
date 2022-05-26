using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Wolfe.FluentCli.Core.Internal;
using Wolfe.FluentCli.Exceptions;
using Wolfe.FluentCli.Parser;
using Wolfe.FluentCli.Parser.Definition;
using Wolfe.FluentCli.Parser.Output;

namespace Wolfe.FluentCli.Core.Builders
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

        public async Task ExecuteAsync(string args)
        {
            var scanner = new CliScanner(args);
            var cliDefinition = CliDefinition.FromCommand(_rootCliCommand);
            var result = _parser.Parse(scanner, cliDefinition);
            var context = BuildContext(result);
            var command = ResolveCommand(context);
            var arguments = ResolveArguments(context, command);
            await ExecuteCoreAsync(context, arguments, command);
        }

        private async Task ExecuteCoreAsync(CliContext context, object arguments, CliCommand command)
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
                args.Add(arguments);

            if (executeMethod.Invoke(handler, args.ToArray()) is Task invokeTask)
                await invokeTask;
        }

        private CliCommand ResolveCommand(CliContext context)
        {
            var currentCommand = _rootCliCommand;
            var currentCommandChain = new List<string>();
            foreach (var commandName in context.Commands)
            {
                var nextCommand = currentCommand.Commands
                    .Find(c => c.Name.Equals(commandName, StringComparison.OrdinalIgnoreCase));
                currentCommand = nextCommand ?? throw new CliInterpreterException($"Unrecognized command chain: {string.Join(' ', currentCommandChain)} {commandName}.");
                currentCommandChain.Add(commandName);
            }

            return currentCommand;
        }

        private static object ResolveArguments(CliContext context, CliCommand command)
        {
            if (command.Arguments == null) { return null; }

            var namedArguments = command.Arguments.Parameters.Where(o => !string.IsNullOrEmpty(o.ShortName));

            // Validate all required arguments have been passed.
            var missingNamedArguments = namedArguments
                .Where(o => o.Required && context.NamedArguments.All(n => n.Name != o.LongName))
                .Select(o => o.LongName).ToList();
            if (missingNamedArguments.Any())
                throw new CliInterpreterException($"The following named arguments are required: {string.Join(' ', missingNamedArguments)}.");

            return command.Arguments.Factory(context);
        }

        private CliContext BuildContext(CliParseResult result)
        {
            var unnamed = new CliArgument(result.Unnamed.Values);
            var named = result.Named.Select(a => new CliNamedArgument(a.Name, a.Values)).ToList();
            return new CliContext(this, result.Commands, unnamed, named);
        }
    }
}
