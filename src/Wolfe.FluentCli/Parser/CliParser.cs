﻿// ReSharper disable SwitchStatementHandlesSomeKnownEnumValuesWithDefault

using System;
using System.Collections.Generic;
using System.Linq;
using Wolfe.FluentCli.Models;
using Wolfe.FluentCli.Parser.Exceptions;
using Wolfe.FluentCli.Parser.Models;

namespace Wolfe.FluentCli.Parser
{
    public class CliParser : ICliParser
    {
        public CliInstruction Parse(ICliScanner scanner, CliDefinition definition)
        {
            var (commands, context) = ParseCommands(scanner, definition);

            CliArgument unnamedArguments = null;
            if (context.Unnamed.AllowedValues != AllowedValues.None)
                unnamedArguments = ParseUnnamedArguments(scanner, definition);

            var namedArguments = ParseNamedArguments(scanner, definition);

            var nextToken = scanner.Read();
            if (nextToken != CliToken.Eof)
                throw new CommandParsingException($"Unexpected token {nextToken.Type}");

            return new CliInstruction
            {
                UnnamedArguments = unnamedArguments,
                Commands = commands,
                NamedArguments = namedArguments
            };
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

        private static CliArgument ParseUnnamedArguments(ICliScanner scanner, CliDefinition definition)
        {
            var values = ParseArgumentValues(scanner, definition.Unnamed.AllowedValues);
            return new CliArgument(values);
        }

        private static List<CliNamedArgument> ParseNamedArguments(ICliScanner scanner, CliDefinition definition)
        {
            var args = new List<CliNamedArgument>();
            while (true)
            {
                var token = scanner.Peek();
                if (token.Type is not (CliTokenType.ShortArgumentMarker or CliTokenType.LongArgumentMarker)) break;
                var argument = ParseNamedArgument(scanner, definition);
                args.Add(argument);
            }
            return args;
        }

        private static CliNamedArgument ParseNamedArgument(ICliScanner scanner, CliDefinition definition)
        {
            // Consume the argument marker.
            var marker = scanner.Read();

            var token = scanner.Read();
            if (token.Type != CliTokenType.Identifier)
                throw new CommandParsingException($"Expected argument identifier, but found {token.Type}");

            var name = token.Value;

            var currentArg = FindArgument(definition, marker.Type, name);
            var values = ParseArgumentValues(scanner, currentArg.AllowedValues);

            return new CliNamedArgument(currentArg.LongName, values);
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
                case AllowedValues.Many: break;
                case AllowedValues.One when values.Count != 1: throw new CommandParsingException("Command requires one unnamed argument.");
                case AllowedValues.None when values.Count != 0: throw new CommandParsingException("Command does not allow unnamed arguments.");
                default: throw new ArgumentOutOfRangeException($"Unexpected {nameof(AllowedValues)} value: {allowedValues}");
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