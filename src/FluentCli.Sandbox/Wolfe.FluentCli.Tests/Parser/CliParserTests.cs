using System.Collections.Generic;
using System.Numerics;
using Wolfe.FluentCli.Parser;
using Wolfe.FluentCli.Parser.Models;
using Wolfe.FluentCli.Tests.Extensions;
using Xunit;

namespace Wolfe.FluentCli.Tests.Parser
{
    public class CliParserTests
    {
        private static readonly CliDefinition ComplexDefinition = new()
        {
            Unnamed = new() { AllowedValues = AllowedValues.Many },
            NamedArguments =
            {
                new() { LongName = "one", ShortName = "o", AllowedValues = AllowedValues.One },
                new() { LongName = "many", ShortName = "m", AllowedValues = AllowedValues.Many }
            },
            Commands =
            {
                new()
                {
                    Aliases = { "f", "foo" },
                    Unnamed = new() { AllowedValues = AllowedValues.One },
                    Commands =
                    {
                        new()
                        {
                            Aliases = { "b", "bar" },
                            Unnamed = new() { AllowedValues = AllowedValues.Many }
                        }
                    }
                },
                new()
                {
                    Aliases = { "b", "bar" },
                    Unnamed = new() { AllowedValues = AllowedValues.None },
                    Commands =
                    {
                        new()
                        {
                            Aliases = { "f", "foo" },
                            Unnamed = new() { AllowedValues = AllowedValues.One }
                        }
                    }
                }
            }
        };

        [Fact]
        public void NestedCommands_ReturnsFirstAlias()
        {
            var scanner = new CliScanner("foo bar");
            var parser = new CliParser();
            var instruction = parser.Parse(scanner, ComplexDefinition);

            Assert.Equal(new List<string> {"f", "b"}, instruction.Commands);
        }

        [Fact]
        public void RootUnnamedArgs_ReturnsArgs()
        {
            var scanner = new CliScanner("random \"unnamed arguments\"");
            var parser = new CliParser();
            var instruction = parser.Parse(scanner, ComplexDefinition);

            Assert.Equal(new List<string> { }, instruction.Commands);
            Assert.Equal(new List<string> { "random", "unnamed arguments"}, instruction.UnnamedArguments.Values);
        }

        [Fact]
        public void CommandWithUnnamedArgs_ReturnsBoth()
        {
            var scanner = new CliScanner("foo combine");
            var parser = new CliParser();
            var instruction = parser.Parse(scanner, ComplexDefinition);

            Assert.Equal(new List<string> { "f" }, instruction.Commands);
            Assert.Equal(new List<string> { "combine" }, instruction.UnnamedArguments.Values);
        }

        [Fact]
        public void NamedArgs_ReturnsArgs()
        {
            var scanner = new CliScanner("-m many args");
            var parser = new CliParser();
            var instruction = parser.Parse(scanner, ComplexDefinition);

            Assert.Equal(new List<string> { }, instruction.Commands);
            Assert.Equal(new List<string> { }, instruction.UnnamedArguments.Values);
            Assert.Equal("many", instruction.NamedArguments[0].Name);
            Assert.Equal(new List<string> { "many", "args" }, instruction.NamedArguments[0].Values);
        }
    }
}
