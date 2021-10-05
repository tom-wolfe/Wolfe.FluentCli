// ReSharper disable SwitchStatementHandlesSomeKnownEnumValuesWithDefault

using System;
using System.Collections.Generic;
using System.Linq;
using Wolfe.FluentCli.Core.Internal;
using Wolfe.FluentCli.Exceptions;
using Wolfe.FluentCli.Parser.Definition;
using Wolfe.FluentCli.Parser.Output;

namespace Wolfe.FluentCli.Parser
{
    internal class CliParser : ICliParser
    {
        public CliParseResult Parse(ICliScanner scanner, CliDefinition definition)
        {
            var (commands, context) = ParseCommands(scanner, definition);

            CliParsedArgument unnamedArguments = null;
            if (context.Unnamed.AllowedValues != AllowedValues.None)
                unnamedArguments = ParseUnnamedArguments(scanner, context);

            var namedArguments = ParseNamedArguments(scanner, context);

            var nextToken = scanner.Read();
            if (nextToken != CliToken.Eof)
                throw new CliInterpreterException($"Unexpected token {nextToken.Type}");

            return new CliParseResult(commands, unnamedArguments, namedArguments);
        }

        private static (List<string>, CliCommandDefinition) ParseCommands(ICliScanner scanner, CliDefinition definition)
        {
            var commands = new List<string>();
            var current = (CliCommandDefinition)definition;
            while (true)
            {
                var token = scanner.Peek();
                if (token.Type != CliTokenType.Identifier) { return (commands, current); }

                var command = FindCommand(current, token.Value);
                if (command == null) { return (commands, current); }

                commands.Add(command.Aliases.First());
                current = command;
                scanner.Read();
            }
        }

        private static CliParsedArgument ParseUnnamedArguments(ICliScanner scanner, CliCommandDefinition definition)
        {
            var values = ParseArgumentValues(scanner, definition.Unnamed.AllowedValues);
            return new CliParsedArgument(values);
        }

        private static List<CliParsedNamedArgument> ParseNamedArguments(ICliScanner scanner, CliCommandDefinition definition)
        {
            var args = new List<CliParsedNamedArgument>();
            while (true)
            {
                var token = scanner.Peek();
                if (token.Type is not (CliTokenType.ShortArgumentMarker or CliTokenType.LongArgumentMarker)) break;
                var argument = ParseNamedArgument(scanner, definition);
                args.Add(argument);
            }
            return args;
        }

        private static CliParsedNamedArgument ParseNamedArgument(ICliScanner scanner, CliCommandDefinition definition)
        {
            // Consume the argument marker.
            var marker = scanner.Read();

            var token = scanner.Read();
            if (token.Type != CliTokenType.Identifier)
                throw new CliInterpreterException($"Expected argument identifier, but found {token.Type}");

            var name = token.Value;

            var currentArg = FindArgument(definition, marker.Type, name);
            var values = ParseArgumentValues(scanner, currentArg.AllowedValues);

            return new CliParsedNamedArgument(currentArg.LongName, values);
        }

        private static List<string> ParseArgumentValues(ICliScanner scanner, AllowedValues allowedValues)
        {
            var values = new List<string>();
            while (true)
            {
                var token = scanner.Peek();
                if (token.Type is not (CliTokenType.Identifier or CliTokenType.StringLiteral)) break;
                values.Add(token.Value);
                scanner.Read();
            }

            switch (allowedValues)
            {
                case AllowedValues.Many:
                    break;
                case AllowedValues.One:
                    if (values.Count != 1) throw new CliInterpreterException("Command requires one unnamed argument.");
                    break;
                case AllowedValues.None:
                    if (values.Count != 0) throw new CliInterpreterException("Command does not allow unnamed arguments.");
                    break;
                default:
                    throw new ArgumentOutOfRangeException($"Unexpected {nameof(AllowedValues)} value: {allowedValues}");
            }

            return values;
        }

        private static CliNamedCommandDefinition FindCommand(CliCommandDefinition command, string alias)
        {
            var match = command.Commands.FirstOrDefault(c => c.Aliases.Contains(alias, StringComparer.OrdinalIgnoreCase));
            return match;
        }

        private static CliNamedArgumentDefinition FindArgument(CliCommandDefinition command, CliTokenType type, string alias)
        {
            var currentArg = command.NamedArguments
                .FirstOrDefault(a => alias.Equals(type == CliTokenType.ShortArgumentMarker ? a.ShortName : a.LongName, StringComparison.OrdinalIgnoreCase));
            return currentArg;
        }
    }
}